using System.Collections.Generic;
using Networking;
using Mirror;
using UnityEngine;

namespace Player
{
	public class CardDeck : NetworkBehaviour
	{
		private Transform GravePosition;
		public Transform DeckPosition;

		public CardEntry FatigueCard;

		private Queue<CardEntry> DeckContent;



		private NetworkIdentity id;

		// Start is called before the first frame update
		void Start()
		{
			id = GetComponent<NetworkIdentity>();
			if (isLocalPlayer)
				CmdSendDeckContent(CardNetworkManager.Instance.CardsInDeck);
		}

		[Command]
		public void CmdSendDeckContent(int[] cardIds)
		{
			//Debug.Log("Setting Content of Deck on Server..");
			SetDeckContent(cardIds); //Set on server
			TargetSetDeckContent(cardIds); //Set On clients
		}

		[TargetRpc]
		public void TargetSetDeckContent(int[] cardIds)
		{
			if (hasAuthority)
			{
				//Debug.Log("Setting Content of Deck on Client..");
				SetDeckContent(CardNetworkManager.Instance.CardsInDeck);
			}
		}

		//Client Sets the content of the deck by ids
		private void SetDeckContent(int[] cardIds)
		{
			DeckContent?.Clear();
			CardEntry[] cardPrefabs = new CardEntry[cardIds.Length];
			for (int i = 0; i < cardPrefabs.Length; i++)
			{
				cardPrefabs[i] = CardNetworkManager.Instance.CardEntries[cardIds[i]];
			}
			DeckContent = new Queue<CardEntry>(cardPrefabs);
		}


		//Server is responsible for drawing/spawning a card
		[Server]
		public CardEntry DrawCard()
		{
			if (DeckContent.Count == 0) return FatigueCard;
			CardEntry e = DeckContent.Dequeue();
			return e;
		}

		[ClientRpc]
		public void RpcSetPositions(Vector3 deckPosition, Vector3 gravePosition)
		{
			SetPositions(deckPosition, gravePosition);
		}

		public void SetPositions(Vector3 deckPosition, Vector3 gravePosition)
		{
			DeckPosition = CreateObject(deckPosition, Quaternion.identity, transform);
			GravePosition = CreateObject(gravePosition, Quaternion.identity, transform);
		}


		public static Transform CreateObject(Vector3 position, Quaternion rotation, Transform parent)
		{
			GameObject obj = new GameObject();
			obj.transform.position = position;
			obj.transform.rotation = rotation;
			obj.transform.parent = parent;
			return obj.transform;
		}



		// Update is called once per frame
		void Update()
		{

		}
	}
}
