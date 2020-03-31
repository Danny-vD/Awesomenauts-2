using System.Collections.Generic;
using Networking;
using Mirror;
using UnityEngine;

public class CardDeck : NetworkBehaviour
{
	private Vector3 GravePosition;
	public Vector3 DeckPosition;
	private Queue<GameObject> DeckContent;

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
		Debug.Log("Setting Content of Deck on Server..");
		SetDeckContent(cardIds); //Set on server
		TargetSetDeckContent(cardIds); //Set On clients
	}

	[TargetRpc]
	public void TargetSetDeckContent(int[] cardIds)
	{
		if (hasAuthority)
		{
			Debug.Log("Setting Content of Deck on Client..");
			SetDeckContent(CardNetworkManager.Instance.CardsInDeck);
		}
	}

	//Client Sets the content of the deck by ids
	private void SetDeckContent(int[] cardIds)
	{
		DeckContent?.Clear();
		GameObject[] cardPrefabs = new GameObject[cardIds.Length];
		for (int i = 0; i < cardPrefabs.Length; i++)
		{
			cardPrefabs[i] = CardNetworkManager.Instance.CardEntries[cardIds[i]].Prefab;
		}
		DeckContent = new Queue<GameObject>(cardPrefabs);
	}


	//Server is responsible for drawing/spawning a card
	[Server]
	public Card DrawCard()
	{
		GameObject cardInstance = Instantiate(DeckContent.Dequeue(), DeckPosition, Quaternion.identity);
		NetworkServer.Spawn(cardInstance, id.connectionToClient);
		return cardInstance.GetComponent<Card>();
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
