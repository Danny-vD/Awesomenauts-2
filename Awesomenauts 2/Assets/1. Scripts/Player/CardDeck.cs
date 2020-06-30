using System.Collections.Generic;
using Networking;
using Mirror;
using UnityEngine;

namespace Player {
	public class CardDeck : NetworkBehaviour
	{
		private Vector3 GravePosition;
		public Vector3 DeckPosition;
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
			CardEntry e = DeckContent.Dequeue();
			return e;
		}

		[TargetRpc]
		public void TargetSetPositions(Vector3 deckPosition, Vector3 gravePosition)
		{
			DeckPosition = deckPosition;
			GravePosition = gravePosition;
		}
		// Update is called once per frame
		void Update()
		{

		}
	}
}
