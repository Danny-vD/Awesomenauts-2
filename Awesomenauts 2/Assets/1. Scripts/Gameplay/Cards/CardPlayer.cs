using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AwsomenautsCardGame.AnimationSystem;
using AwsomenautsCardGame.DataObjects.Networking;
using AwsomenautsCardGame.Enums.Cards;
using AwsomenautsCardGame.Maps;
using AwsomenautsCardGame.Networking;
using AwsomenautsCardGame.ScriptableObjects.DragLogic;
using AwsomenautsCardGame.ScriptableObjects.Effects;
using AwsomenautsCardGame.UI;
using AwsomenautsCardGame.UI.DebugPanel;
using AwsomenautsCardGame.UI.TooltipSystem;
using Mirror;
using UnityEngine;

namespace AwsomenautsCardGame.Gameplay.Cards
{

	public class LastGameInformation
	{
		public int WinnerID = -1;
		public int LoserID = -1;
		public int LocalID = -1;
		public int TotalRounds = 0;
	}

	public class CardPlayer : NetworkBehaviour
	{

		public static LastGameInformation LastGame { get; private set; }

		public CameraController CameraController;

		public EntityStatistics PlayerStatistics => Awsomenaut?.Statistics;
		[HideInInspector]
		public Card Awsomenaut;

		private List<Card> UsedCardsThisTurn = new List<Card>();
		private List<Card> PreviewCards = new List<Card>();


		public void ClearUsedCards() { UsedCardsThisTurn.Clear(); }
		public bool CanUseCard(Card c) => !UsedCardsThisTurn.Contains(c);


		public CardDragLogic DragFromHandLogic;
		public CardDragLogic DragEffectFromHandLogic;
		public CardDragLogic DragNoTargetEffectFromHandLogic;

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

		public LayerMask SocketLayer;
		public LayerMask BoardLayer;
		public LayerMask CardDragLayer;
		private LayerMask PlayerHandLayer => Hand.PlayerHandLayer;
		private LayerMask AllCardLayers => BoardLayer.value | PlayerHandLayer.value;

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
		private void Awake()
		{
			Camera = Camera.main;
		}

		private void Start()
		{
			if (isClient && hasAuthority)
			{
				LocalPlayer = this;
				LastGame = new LastGameInformation();
				Camera.transform.parent = transform;
			}
			else
			{
				CameraController.enabled = false;
			}
			if (isServer)
			{
				ClientID = ServerPlayers.Count;
				ServerPlayers.Add(this);
			}
		}

		[Command]
		private void CmdSetReady(int id)
		{
			SetReady(id);
		}

		private void SetReady(int id)
		{
			Debug.Log("Client: " + id + " is ready.");
			IsReady = true;
			if (id != ClientID)
			{
				ExceptionViewUI.Instance.SetException(new Exception("ClientIDMismatch"), "Client ID Missmatch");
				return;
			}
			BoardLogic.Logic.SetClientReady(ClientID);
		}

		[ClientRpc]
		public void RpcSetClientPlayerID(int id, int[] clientIds)
		{
			SetClientPlayerId(id, clientIds);
		}

		private void SetClientPlayerId(int id, int[] clientIds)
		{
			ClientID = id;
			if (ServerPlayers.Count <= id) ServerPlayers.Add(this);
			else
			{
				ServerPlayers[id] = this;
			}

			StartCoroutine(WaitForMapRoutine(id, clientIds));
		}

		private IEnumerator WaitForMapRoutine(int id, int[] clientIds)
		{
			while (MapTransformInfo.Instance == null) yield return new WaitForEndOfFrame();


			MapTransformInfo.Instance.SocketManager.AddPlayers(clientIds);

			Debug.Log("Client: " + id + " is ready.");
			if (hasAuthority)
			{
				CmdSetReady(id);
			}
			yield return null;
		}

		[ClientRpc]
		public void RpcSetAwsomenaut(NetworkIdentity id)
		{
			SetAwsomenaut(id);
		}

		public void SetAwsomenaut(NetworkIdentity id)
		{
			Awsomenaut = id.GetComponent<Card>();
			Awsomenaut.Statistics.Register(CardPlayerStatType.HP, OnHPChange);
			//OnHPChange
		}

		private void OnDestroy()
		{
			if (Awsomenaut != null)
				Awsomenaut.Statistics.UnregisterAll();
		}

		private void OnHPChange(object newvalue)
		{
			if (this == null) return;
			if (newvalue is int i)
			{
				if (i <= 0)
				{
					StartCoroutine(EndGame());
				}
			}
		}


		private IEnumerator EndGame()
		{
			LastGame.LoserID = ClientID;
			LastGame.WinnerID = ServerPlayers.First(x => x.ClientID != ClientID).ClientID;
			LastGame.TotalRounds = BoardLogic.Logic.TotalTurns / ServerPlayers.Count;
			LastGame.LocalID = LocalPlayer.ClientID;
			//Play Sound
			ServerPlayers.ForEach(x => x.EnableInteractions = false);
			yield return new WaitForSeconds(1);

			//Log Statistics


			CardNetworkManager.Instance.Stop();
		}

		// Update is called once per frame
		private void Update()
		{
			if (!DebugPanelInfo.Instance || LocalPlayer == null) return;
			bool hoverCard = IsHoveringCard(AllCardLayers, out RaycastHit chit);
			DebugPanelInfo.Instance.CardPreviewCameraImage.enabled = hoverCard;
			if (hoverCard)
			{
				Card c = chit.transform.GetComponent<Card>();

				PreviewCards.ForEach(x => x.SetPreviewLayer(false));
				PreviewCards.Clear();
				PreviewCards.Add(c);
				c.SetPreviewLayer(true);

				DebugPanelInfo.Instance.CardPreviewCamera.transform.position = new Vector3(c.transform.position.x,
					c.transform.position.y + 6.5f, c.transform.position.z);
				DebugPanelInfo.Instance.CardPreviewCamera.transform.rotation = c.transform.rotation;
				DebugPanelInfo.Instance.CardPreviewCamera.transform.Rotate(Vector3.right,
					180); //From looking up to looking on the card (down)
				DebugPanelInfo.Instance.CardPreviewCamera.transform.Rotate(Vector3.forward,
					180); //Rotating so that the card is rotated correctly relative to the camera

				if (c.gameObject.layer == 8) //CardHandLayer
				{
					DebugPanelInfo.Instance.CardPreviewCamera.transform.Translate(0, -3.5f, 0, Space.Self);
				}
			}

			if (EnableInteractions)
			{
				DragCard(hoverCard, chit);
			}
		}


		[Server]
		public void DrawCard(int amount)
		{
			int cardsToDraw = Mathf.Min(Hand.CardSlotsFree, amount);
			if (cardsToDraw != amount)
			{
				//Debug.Log("Adding To many cards to the hand, the maximum is: " + Hand.MaxCardCount);
			}


			//Debug.Log("Adding " + cardsToDraw + " cards to the hand of client: " +
			//netIdentity.connectionToClient.connectionId);

			for (int i = 0; i < cardsToDraw; i++)
			{
				CardEntry e = Deck.DrawCard();
				DrawCard(e);
			}
		}

		[Server]
		public void DrawCard(CardEntry e)
		{
			Card c = CreateCard(e);
			//Hand.AddToHand(c.GetComponent<NetworkIdentity>());
			Hand.RpcAddToHand(c.netIdentity); //Add Card to the client
		}

		[Server]
		public Card CreateCard(CardEntry e)
		{
			e.Statistics.InitializeStatDictionary();
			//Hand.AddCard(c);//Add the Card to the server
			GameObject cardInstance = Instantiate(e.Prefab, Deck.DeckPosition.position, Quaternion.identity);
			Card c = cardInstance.GetComponent<Card>();
			GameObject modelPrefab = e.Model.Get(ClientID);
			if (modelPrefab != null)
			{
				GameObject model = Instantiate(modelPrefab, c.Model.position, modelPrefab.transform.rotation);
				model.transform.SetParent(c.Model, true);
				//model.transform.parent = c.Model;
				if (c.Animator == null)
				{
					c.Animator = c.Model.GetComponentInChildren<AnimationPlayer>();
				}

			}
			NetworkServer.Spawn(cardInstance, GetComponent<NetworkIdentity>().connectionToClient);

			c.Statistics = e.Statistics;
			c.EffectManager = new EffectManager(e.effects ?? new List<AEffect>());
			c.Statistics.SetValue(CardPlayerStatType.TeamID, ClientID); //Set Team ID, used to find out to whom the card belongs.
			c.Statistics.SetValue(CardPlayerStatType.CardType, e.CardType); //Set Team ID, used to find out to whom the card belongs.
			byte[] networkData = e.StatisticsToNetworkableArray();
			//Debug.Log("Sending Stats");
			//Debug.Log("Card Type: " + c.Statistics.GetValue(CardPlayerStatType.CardType));
			c.RpcSendStats(networkData, e.effects.Select(x => CardNetworkManager.Instance.AllEffects.IndexOf(x)).ToArray());
			return c;
		}

		[Client]
		public void EndTurn()
		{
			//Debug.Log("Trying to end Turn as Client: " + ClientID);
			CmdEndTurn(ClientID);
		}

		[Command]
		private void CmdEndTurn(int clientID)
		{
			if (BoardLogic.Logic.CurrentTurnClient == clientID) BoardLogic.Logic.ServerEndTurn();
		}

		[ClientRpc]
		public void RpcSetCameraPosition(Vector3 pos, Quaternion rot, bool invertValues)
		{
			if (hasAuthority)
				SetCameraPosition(pos, rot, invertValues);
		}

		private void SetCameraPosition(Vector3 pos, Quaternion rot, bool invertValues)
		{
			if (Camera == null) return;
			Camera.transform.SetPositionAndRotation(pos, rot);
			CameraController.SetOriginalPosition(transform.position, invertValues);
			if (invertValues)
			{
				invertCardRotation = true;
				Hand.InvertValues();
			}
		}

		#region DragCardFromHand

		private void HandleDraggingCardFromHand()
		{
			CardDragLogic logic = draggedCard.CardType == CardType.Action ? DragEffectFromHandLogic : DragFromHandLogic;
			logic = draggedCard.CardType == CardType.ActionNoTarget ? DragNoTargetEffectFromHandLogic : logic;
			Vector3 dir = GetCardPosition(draggedCard, logic) - draggedCard.transform.position;
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

		private void HandleReleasedCardFromHand()
		{
			if (waitForRelease) return;
			waitForRelease = true;
			dragging = false;
			DebugPanelInfo.Instance.SolarDisp.SetSolarCost(0);
			if (!snapping)
			{
				Hand.SetSelectedCard(null); //Return card to hand
				waitForRelease = false;
			}
			else
			{
				Hand.SetSelectedCard(null); //Return card to hand
				CmdPlaceCard(draggedCard.netIdentity, SnappedSocket.netIdentity);
			}
			//Debug.Log("Released Card");

		}

		[Command]
		private void CmdPlaceCard(NetworkIdentity identity, NetworkIdentity socket)
		{
			RpcPlaceCard(identity, socket);
		}

		[ClientRpc]
		private void RpcPlaceCard(NetworkIdentity draggedCardIdentity, NetworkIdentity socket)
		{
			PlaceCard(draggedCardIdentity, socket);
			waitForRelease = false;
		}

		private void PlaceCard(NetworkIdentity draggedCardIdentity, NetworkIdentity socket)
		{
			//Remove Available Solar Client Side
			Card c = draggedCardIdentity.GetComponent<Card>();
			CardSocket cs = socket.GetComponent<CardSocket>();

			UsedCardsThisTurn.Add(c); //Card that got placed is not able to do any additional action in the turn

			int solar = PlayerStatistics.GetValue<int>(CardPlayerStatType.Solar);
			int sub = c.Statistics.GetValue<int>(CardPlayerStatType.Solar);
			solar -= sub;
			PlayerStatistics.SetValue(CardPlayerStatType.Solar, solar);
			c.EffectManager.InvokeEffects(EffectTrigger.OnPlay, null, cs, c);

			//Place card on board
			//CmdRemoveFromHand(c.GetComponent<NetworkIdentity>());
			c.gameObject.layer = UnityTrashWorkaround(BoardLayer);
			Hand.RemoveCard(c.GetComponent<Card>());

			if (c.Statistics.GetValue<CardType>(CardPlayerStatType.CardType) == CardType.Action || c.Statistics.GetValue<CardType>(CardPlayerStatType.CardType) == CardType.ActionNoTarget)
			{
				Destroy(c.gameObject);
				return;
			}


			//We Have to turn the card facing up again
			Quaternion turnOverRot = Quaternion.AngleAxis(180, c.transform.right);
			c.transform.rotation *= turnOverRot;
			c.SetState(CardState.OnBoard);

			if (cs.hasAuthority)
			{
				cs.CmdDockCard(c.netIdentity);
			}
			else
			{
				cs.DockCard(c);
			}

			if (c.hasAuthority)
			{
				Hand.SetSelectedCard(null);
			}
			c.EffectManager.InvokeEffects(EffectTrigger.AfterPlay, cs, null, c);
		}

		private void HandleClickedOnCardOnHand(Card c, bool fromHand)
		{
			if (!fromHand) return;

			canSnap = true;
			if (c.Statistics.HasValue(CardPlayerStatType.Solar))
			{
				int playerSolar = PlayerStatistics.GetValue<int>(CardPlayerStatType.Solar);
				int cardSolar = c.Statistics.GetValue<int>(CardPlayerStatType.Solar);
				if (cardSolar > playerSolar)
				{
					TooltipScript.Instance.SetTooltip(TooltipType.NotEnoughSolar);
					canSnap = false;
					//return; //Not enough Solar
				}
				else
				{
					DebugPanelInfo.Instance.SolarDisp.SetSolarCost(cardSolar);
					TooltipScript.Instance.SetTooltip(TooltipType.CardPlayAccepted);
					Hand.SetSelectedCard(c);
					canSnap = true;
				}
			}
			else //Card has no solar costs or nothing defined as solar cost
			{
				Hand.SetSelectedCard(c);
				canSnap = true;
			}

			dragging = true;
			draggedCard = c;
			//Debug.Log("Clicked On Card");
		}

		#endregion

		#region DragCardFromBoard



		public ArrowDisplay ArrowDisplayHelper;
		private void HandleClickedOnCardOnBoard(Card c)
		{

			if (c.Statistics.GetValue<int>(CardPlayerStatType.TeamID) == ClientID) //Card belongs to us
			{

				draggedCard = c;
				dragging = true;
			}
		}

		private void HandleReleasedCardFromBoard(NetworkIdentity draggedCardSocket, NetworkIdentity targetCardSocket)
		{
			if (draggedCardSocket == null || targetCardSocket == null)
			{
				draggedCard = null;
				dragging = false;
				snapping = false;
				SnappedSocket = null;
				canSnap = false;
				Hand.SetSelectedCard(null);

				return;
			}
			CardSocket draggedSocket = draggedCardSocket.GetComponent<CardSocket>();
			Card sourceCard = draggedSocket.DockedCard;
			CardSocket targetSocket = targetCardSocket.GetComponent<CardSocket>();
			if (sourceCard == null) return;
			if (sourceCard.DragLogicFromBoard.CanTarget(this, targetSocket, sourceCard.AttachedCardSocket))
			{
				CardAction action =
					sourceCard.DragLogicFromBoard.GetAction(this, targetSocket, sourceCard.AttachedCardSocket);
				if (action == CardAction.Attack)
				{
					UsedCardsThisTurn.Add(sourceCard);
					Card targetCard = targetSocket.DockedCard;
					sourceCard.EffectManager.InvokeEffects(EffectTrigger.OnAttacking, sourceCard.AttachedCardSocket, targetSocket, sourceCard);
					//targetCard.EffectManager.InvokeEffects(EffectTrigger.OnAttacked, targetSocket, sourceCard.AttachedCardSocket, targetCard);
					sourceCard.Attack(targetSocket.DockedCard);
					sourceCard.EffectManager.InvokeEffects(EffectTrigger.AfterAttacking, sourceCard.AttachedCardSocket, targetSocket, sourceCard);
					targetCard.EffectManager.InvokeEffects(EffectTrigger.AfterAttacked, targetSocket, sourceCard.AttachedCardSocket, targetCard);
				}
				else if (action == CardAction.Move)
				{
					//Debug.Log("MOVE");
					UsedCardsThisTurn.Add(sourceCard);
					sourceCard.EffectManager.InvokeEffects(EffectTrigger.OnMove, sourceCard.AttachedCardSocket, targetSocket, sourceCard);

					StartCoroutine(WaitForCard(sourceCard, () =>
					{
						if (sourceCard.AttachedCardSocket != null)
						{
							if (sourceCard.AttachedCardSocket.hasAuthority)
							{
								sourceCard.AttachedCardSocket.CmdUnDockCard();
							}
							else
							{
								sourceCard.AttachedCardSocket.DockCard(null);
							}
						}

						sourceCard.AttachedCardSocket?.CmdUnDockCard();
						if (targetSocket.hasAuthority)
						{
							targetSocket.CmdDockCard(sourceCard.netIdentity);
						}
						else
						{
							targetSocket.DockCard(sourceCard);
						}
						sourceCard.EffectManager.InvokeEffects(EffectTrigger.AfterMove, sourceCard.AttachedCardSocket, targetSocket, sourceCard);

					}));

				}
			}

			ArrowDisplayHelper.Deactivate();
			dragging = false;
			draggedCard = null;


		}


		private IEnumerator WaitForCard(Card c, Action a)
		{
			while (c.IsLocked)
			{
				yield return new WaitForEndOfFrame();
			}

			a();
		}

		[ClientRpc]
		private void RpcHandleReleasedCardFromBoard(NetworkIdentity draggedCardSocket, NetworkIdentity cardSocket)
		{
			HandleReleasedCardFromBoard(draggedCardSocket, cardSocket);
			waitForRelease = false;
		}
		[Command]
		private void CmdHandleReleasedCardFromBoard(NetworkIdentity draggedCardSocket, NetworkIdentity cardSocket)
		{
			RpcHandleReleasedCardFromBoard(draggedCardSocket, cardSocket);
		}

		private bool waitForRelease;
		private void HandleReleasedCardFromBoard()
		{
			if (waitForRelease) return;
			waitForRelease = true;
			if (HasPlacedCard(out RaycastHit info, out CardSocket s))
			{
				CmdHandleReleasedCardFromBoard(draggedCard?.AttachedCardSocket?.netIdentity, s?.netIdentity);
			}
			else
			{
				draggedCard = null;
				dragging = false;
				snapping = false;
				SnappedSocket = null;
				canSnap = false;
				Hand.SetSelectedCard(null);
				waitForRelease = false;
				ArrowDisplayHelper.SetArrowPositions(Vector3.zero, Vector3.zero, ArrowDisplayState.None);
				return;
			}

		}

		private void HandleDraggingCardFromBoard()
		{
			if (!dragging) return;


			Vector3 arrowPos;
			if (IsHoveringCard(SocketLayer, out RaycastHit info))
			{
				arrowPos = info.collider.transform.position;
				if (draggedCard.DragLogicFromBoard.CanTarget(this, info.collider.GetComponent<CardSocket>(), draggedCard.AttachedCardSocket))
				{
					ArrowDisplayHelper.SetArrowPositions(draggedCard.transform.position, arrowPos, ArrowDisplayState.Accept);
				}
				else
				{
					ArrowDisplayHelper.SetArrowPositions(draggedCard.transform.position, arrowPos, ArrowDisplayState.Invalid);
				}
			}
			else
			{
				arrowPos = GetMousePositionOnDragLayer();
				ArrowDisplayHelper.SetArrowPositions(draggedCard.transform.position, arrowPos, ArrowDisplayState.Default);
			}
		}

		#endregion


		private Card hoveredCard = null;
		private float hoverTimeStamp;

		private void HoverCard(Card c, bool isPrevious, bool fromOwnTeam)
		{
			if (!isPrevious) //If this is a new card
			{
				if (hoveredCard != null)
					hoveredCard.transform.localScale = Vector3.one; //Reset if we hovered a card previously
				if (fromOwnTeam) //Check if the card is from the hand
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

		private void DragCard(bool hoverCard, RaycastHit chit)
		{
			bool clicked = Input.GetMouseButton(0);
			Card c = null;
			if (hoverCard) c = chit.transform.GetComponent<Card>();
			bool fromHand = c != null && Hand.IsCardFromHand(c);
			bool fromOwnTeam = c != null && c.StatisticsValid &&
							   c.Statistics.GetValue<int>(CardPlayerStatType.TeamID) == ClientID;
			bool isPrevious = hoveredCard != null && c == hoveredCard;


			if (hoverCard)
			{

				HoverCard(c, isPrevious, fromOwnTeam);

			}
			else
			{
				if (hoveredCard != null)
				{
					hoveredCard.transform.localScale = Vector3.one; //Reset if we hovered a new card }
					hoveredCard = null;
				}
			}


			if (!dragging && hoverCard && clicked) //OnCardDrag Start
			{

				switch (c.CardState)
				{
					case CardState.OnDeck:
						break;
					case CardState.OnHand:
						HandleClickedOnCardOnHand(c, fromHand);
						break;
					case CardState.OnBoard:
						HandleClickedOnCardOnBoard(c);
						break;
					case CardState.OnGrave:
						break; //DoNothing
				}
			}
			else if (dragging && !clicked /*&& draggedCard != null*/) //Card Released
			{
				switch (draggedCard.CardState)
				{
					case CardState.OnDeck:
						break;
					case CardState.OnHand:
						HandleReleasedCardFromHand();
						break;
					case CardState.OnBoard:
						HandleReleasedCardFromBoard();
						break;
					case CardState.OnGrave:
						break; //DoNothing
				}
			}
			else if (dragging && clicked && draggedCard != null) //Card Dragging
			{
				switch (draggedCard.CardState)
				{
					case CardState.OnDeck:
						break;
					case CardState.OnHand:
						HandleDraggingCardFromHand();
						break;
					case CardState.OnBoard:
						HandleDraggingCardFromBoard();
						break;
					case CardState.OnGrave:
						break; //DoNothing
				}

			}
		}

		private Vector3 GetCardPosition(Card c, CardDragLogic logic)
		{

			if (canSnap &&
				HasPlacedCard(out RaycastHit socketPlace, out CardSocket s) &&
				logic.CanTarget(this, s, c.AttachedCardSocket))
			{
				snapping = true;
				SnappedSocket = s;
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
			//Debug.Log("DragLayer: " + LayerMask.LayerToName(UnityTrashWorkaround(CardDragLayer)));
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


		private bool IsHoveringCard(LayerMask mask, out RaycastHit info)
		{
			Ray r = Camera.ScreenPointToRay(Input.mousePosition);
			info = new RaycastHit();
			bool ret = Physics.Raycast(r, out info, float.MaxValue, mask);
			return ret;
		}



		private bool IsHoveringCardExcept(LayerMask mask, out RaycastHit info, params Collider[] ignoredColliders)
		{
			Ray r = Camera.ScreenPointToRay(Input.mousePosition);
			info = new RaycastHit();
			RaycastHit[] hits = Physics.RaycastAll(r, float.MaxValue, mask);
			for (int i = 0; i < hits.Length; i++)
			{
				if (!ignoredColliders.Contains(hits[i].collider))
				{
					info = hits[i];
					return true;
				}
			}
			info = new RaycastHit();
			return false;
		}

		private bool HasPlacedCard(out RaycastHit info, out CardSocket socket)
		{
			Ray r = Camera.ScreenPointToRay(Input.mousePosition);
			info = new RaycastHit();
			bool ret = Physics.Raycast(r, out info, float.MaxValue, SocketLayer);
			socket = ret ? info.transform.GetComponent<CardSocket>() : null;
			return ret;
			//	  && MapTransformInfo.Instance.SocketManager.IsFromTeam(ClientID, info.transform);
		}
	}
}