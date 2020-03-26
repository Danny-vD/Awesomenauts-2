using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Networking.Types;

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
        SetDeckContent(CardNetworkManager.Instance.CardsInDeck);
    }

    //Client Sets the content of the deck by ids
    public void SetDeckContent(int[] cardIds)
    {
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
