using System;
using System.Collections.Generic;
using Mirror;
using Networking;
using UnityEngine;


public class CardPlayer : NetworkBehaviour
{
	public EntityStatistics PlayerStatistics;

	public static CardPlayer LocalPlayer;
	public static List<CardPlayer> ServerPlayers = new List<CardPlayer>();

	public static bool AllPlayersReady
	{
		get
		{
			bool allready = true;
			ServerPlayers.ForEach(x => allready &= x.IsReady);
			return allready;
		}
	}

	public bool EnableInteractions = false;
	public bool IsReady { get; private set; }

	public CardHand Hand;
	public CardDeck Deck;

	public LayerMask Socket;
	private LayerMask PlayerHandLayer => Hand.PlayerHandLayer;
	public LayerMask CardDragLayer;

	[Range(0, 1f)]
	public float DragWhenMoving = 0.7f;

	[Range(0, 1f)]
	public float DragWhenSnapping = 0.1f;

	private float Drag => snapping ? DragWhenSnapping : DragWhenMoving;

	[Range(0, 2)]
	public float DragIntertiaMultiplier = 1;


	[Range(0f, 3f)]
	public float AdditionalHoveredCardScale = 1;

	[Range(0.01f, 3f)]
	public float CardHoverAnimationTime = 1;

	[Range(0, 50)]
	public float MaxIntertia = 25;

	private Camera Camera;
	private bool snapping;
	private bool canSnap;
	private CardSocket SnappedSocket;
	private bool dragging;
	private Card draggedCard;
	public Vector3 CardCounterRotation;
	private bool invertCardRotation;

	public int ClientID { get; private set; }

	// Start is called before the first frame update
	private void Start()
	{
		PlayerStatistics.InitializeStatDictionary();
		Camera = Camera.main;
		if (isLocalPlayer)
		{
			LocalPlayer = this;


			//Got replaced by a non dynamic function BoardLogic.ClientRequestEndTurn()
			//DebugPanelInfo dpi = FindObjectOfType<DebugPanelInfo>();
			//if (dpi != null)
			//{
			//    dpi.RegisterEndTurn(EndTurn);
			//}
			CmdRequestClientID();
		}

		ServerPlayers.Add(this);
		//Debug.Log($"isLocalPlayer: {isLocalPlayer}\nIsClient: {isClient}\nIsServer: {isServer}");
	}

	[Command]
	private void CmdRequestClientID()
	{
		ClientID = GetComponent<NetworkIdentity>().connectionToClient.connectionId;
		Debug.Log("ServerSide Client ID Set to: " + ClientID);
		TargetSetClientID(GetComponent<NetworkIdentity>().connectionToClient, ClientID);
	}

	[Command]
	private void CmdSetReady()
	{
		Debug.Log("Client is ready.");
		IsReady = true;
	}

	[TargetRpc]
	private void TargetSetClientID(NetworkConnection conn, int clientID)
	{
		ClientID = clientID;
		Debug.Log("Client ID Set to: " + ClientID);

		CmdSetReady();
		IsReady = true;
	}

	// Update is called once per frame
	private void Update()
	{
		if (EnableInteractions)
		{
			DragCard();
		}
	}

	[Server]
	public void DrawCard(int amount)
	{
		int cardsToDraw = Mathf.Min(Hand.CardSlotsFree, amount);
		if (cardsToDraw != amount)
		{
			Debug.Log("Adding To many cards to the hand, the maximum is: " + Hand.MaxCardCount);
		}


		Debug.Log("Adding " + cardsToDraw + " cards to the hand of client: " +
				  netIdentity.connectionToClient.connectionId);

		for (int i = 0; i < cardsToDraw; i++)
		{
			CardEntry e = Deck.DrawCard();
			//Hand.AddCard(c);//Add the Card to the server
			GameObject cardInstance = Instantiate(e.Prefab, Deck.DeckPosition, Quaternion.identity);
			NetworkServer.Spawn(cardInstance, GetComponent<NetworkIdentity>().connectionToClient);
			Card c = cardInstance.GetComponent<Card>();
			c.Statistics = e.Statistics;
			Tuple<int[], int[], string[]> networkData = e.StatisticsToNetworkableTypes();
			Debug.Log("Sending Stats");
			c.TargetSendStats(netIdentity.connectionToClient, networkData.Item1, networkData.Item2, networkData.Item3);

			Hand.AddToHand(c.GetComponent<NetworkIdentity>());
			Hand.TargetAddToHand(netIdentity.connectionToClient,
				c.GetComponent<NetworkIdentity>()); //Add Card to the client
		}
	}

	[Client]
	public void EndTurn()
	{
		Debug.Log("Trying to end Turn as Client: " + ClientID);
		CmdEndTurn(ClientID);
	}

	[Command]
	private void CmdEndTurn(int clientID)
	{
		if (BoardLogic.Logic.CurrentTurnClient == clientID) BoardLogic.Logic.ServerEndTurn();
	}

	[TargetRpc]
	public void TargetSetCameraPosition(NetworkConnection target, Vector3 pos, Quaternion rot, bool InvertCardValues)
	{
		Camera.transform.SetPositionAndRotation(pos, rot);
		if (InvertCardValues)
		{
			invertCardRotation = true;
			Hand.InvertValues();
		}
	}

	[Command]
	private void CmdRemoveFromHand(NetworkIdentity id)
	{
		Debug.Log("Card NULL: " + (id.GetComponent<Card>() == null));

		if (!CardNetworkManager.Instance.IsHost
		) //To avoid reducing solar twice(on client side and host side when a client is hosting)
		{
			int solar = PlayerStatistics.GetValue<int>(CardPlayerStatType.Solar);
			int sub = id.GetComponent<Card>().Statistics.GetValue<int>(CardPlayerStatType.Solar);
			solar -= sub;
			PlayerStatistics.SetValue(CardPlayerStatType.Solar, solar);
		}

		Hand.RemoveCard(id.GetComponent<Card>());
	}

	#region DragCardFromHand

	private void HandleReleasedCardFromHand()
	{
		dragging = false;
		if (!snapping)
		{
			Hand.SetSelectedCard(null); //Return card to hand
		}
		else
		{
			//Remove Available Solar Client Side
			int solar = PlayerStatistics.GetValue<int>(CardPlayerStatType.Solar);
			int sub = draggedCard.Statistics.GetValue<int>(CardPlayerStatType.Solar);
			solar -= sub;
			PlayerStatistics.SetValue(CardPlayerStatType.Solar, solar);

			//Place card on board
			CmdRemoveFromHand(draggedCard.GetComponent<NetworkIdentity>());
			Hand.RemoveCard(draggedCard.GetComponent<Card>());

			//We Have to turn the card facing up again
			Quaternion turnOverRot = Quaternion.AngleAxis(180, draggedCard.transform.right);
			draggedCard.transform.rotation *= turnOverRot;

			SnappedSocket.DockCard(draggedCard);
			draggedCard.SetState(CardState.OnBoard);
			Hand.SetSelectedCard(null);
		}

		Debug.Log("Released Card");
	}

	private void HandleClickedOnCardOnHand(Card c)
	{
		if (Hand.IsCardFromHand(c))
			Hand.SetSelectedCard(c);
		else return;
		canSnap = true;
		if (c.Statistics.HasValue(CardPlayerStatType.Solar))
		{
			int playerSolar = PlayerStatistics.GetValue<int>(CardPlayerStatType.Solar);
			int cardSolar = c.Statistics.GetValue<int>(CardPlayerStatType.Solar);
			if (cardSolar > playerSolar)
			{
				canSnap = false;
				//return; //Not enough Solar
			}
			else
			{
				canSnap = true;
			}
		}
		else //Card has no solar costs or nothing defined as solar cost
		{
			canSnap = true;
		}

		dragging = true;
		draggedCard = c;
		Debug.Log("Clicked On Card");
	}

	#endregion


	private Card hoveredCard = null;
	private float hoverTimeStamp;

	private void DragCard()
	{
		if (!dragging)
		{
			//IF is hovering
			//Make Scale Bigger
			//when card changed or no card hovered reset old card scale
			if (IsHoveringCard(out RaycastHit chit))
			{
				Card c = chit.transform.GetComponent<Card>();

				bool isPrevious = hoveredCard != null && c == hoveredCard;

				if (!isPrevious) //If this is a new card
				{
					if (hoveredCard != null)
						hoveredCard.transform.localScale = Vector3.one; //Reset if we hovered a card previously
					if (Hand.IsCardFromHand(c)) //Check if the card is from the hand
					{
						hoverTimeStamp = Time.realtimeSinceStartup; //Update the Timestamp
						hoveredCard = c; //Set the hovered Card.
					}
				}

				if (hoveredCard != null)
				{
					float t = Time.realtimeSinceStartup - hoverTimeStamp;
					float x = Mathf.Clamp01(t / CardHoverAnimationTime);
					hoveredCard.transform.localScale = Vector3.one * (1 + x * AdditionalHoveredCardScale);
				}

			}
			else
			{
				if (hoveredCard != null)
				{
					hoveredCard.transform.localScale = Vector3.one; //Reset if we hovered a new card }
					hoveredCard = null;
				}
			}

			if (HasClickedOnCard(out RaycastHit cardHit))
			{
				if (hoveredCard != null)
				{
					hoveredCard.transform.localScale = Vector3.one; //Reset if we hovered a new card }
					hoveredCard = null;
				}

				Card c = cardHit.transform.GetComponent<Card>();


				switch (c.CardState)
				{
					case CardState.OnDeck:
						break;
					case CardState.OnHand:
						HandleClickedOnCardOnHand(c);
						break;
					case CardState.OnBoard:
						break;
					case CardState.OnGrave:
						break; //DoNothing
				}
			}
		}
		else
		{
			if (!Input.GetMouseButton(0))
			{
				switch (draggedCard.CardState)
				{
					case CardState.OnDeck:
						break;
					case CardState.OnHand:
						HandleReleasedCardFromHand();
						break;
					case CardState.OnBoard:
						break;
					case CardState.OnGrave:
						break; //DoNothing
				}
			}
			else
			{
				Vector3 dir = GetCardPosition() - draggedCard.transform.position;
				float m = Mathf.Clamp(dir.magnitude * DragIntertiaMultiplier, 0, MaxIntertia);

				dir *= Drag;
				draggedCard.transform.position += dir;
				Vector3 axis = Vector3.Cross(Vector3.up, dir);
				Quaternion q = Quaternion.AngleAxis(m, axis);


				draggedCard.transform.rotation = q;
				Quaternion turnOverRot = Quaternion.AngleAxis(180, draggedCard.transform.right);
				draggedCard.transform.rotation *= turnOverRot;
				Vector3 euler = new Vector3(CardCounterRotation.x,
					invertCardRotation ? -CardCounterRotation.y : CardCounterRotation.y, CardCounterRotation.z);
				Quaternion c = Quaternion.Euler(euler);
				draggedCard.transform.rotation *= c;
			}
		}
	}

	private Vector3 GetCardPosition()
	{
		if (canSnap && HasPlacedCard(out RaycastHit socketPlace))
		{
			snapping = true;
			SnappedSocket = socketPlace.transform.GetComponent<CardSocket>();
			Vector3 socketPos = socketPlace.transform.position + Vector3.up;
			return socketPos;
		}

		snapping = false;
		SnappedSocket = null;
		return GetMousePositionOnDragLayer();
	}


	private Vector3 GetMousePositionOnDragLayer()
	{
		Ray r = Camera.ScreenPointToRay(Input.mousePosition);
		Debug.Log("DragLayer: " + LayerMask.LayerToName(UnityTrashWorkaround(CardDragLayer)));
		if (Physics.Raycast(r, out RaycastHit info, float.MaxValue, CardDragLayer))
		{
			return info.point;
		}

		return Vector3.zero;
	}

	public static int UnityTrashWorkaround(LayerMask lm)
	{
		int m = lm.value;
		int i = 1;
		if (lm == 0) return 0;
		while ((m = m >> 1) != 1)
		{
			i++;
		}

		return i;
	}

	private bool HasClickedOnCard(out RaycastHit info)
	{
		if (Input.GetMouseButtonDown(0) && IsHoveringCard(out info))
		{
			return true;
		}

		info = new RaycastHit();
		return false;
	}

	private bool IsHoveringCard(out RaycastHit info)
	{
		Ray r = Camera.ScreenPointToRay(Input.mousePosition);
		info = new RaycastHit();
		bool ret = Physics.Raycast(r, out info, float.MaxValue, PlayerHandLayer);
		return ret;
	}

	private bool HasPlacedCard(out RaycastHit info)
	{
		Ray r = Camera.ScreenPointToRay(Input.mousePosition);
		info = new RaycastHit();
		return Physics.Raycast(r, out info, float.MaxValue, Socket) &&
			   MapTransformInfo.Instance.SocketManager.IsFromTeam(ClientID, info.transform);
	}
}