using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGameBoard : MonoBehaviour, IGameBoard
{
	public GameSettingsObject GameSettings;
	public CardPlayer playerBlue;
	public IPlayer PlayerBlue => playerBlue;

	public CardPlayer playerRed;
	public IPlayer PlayerRed => playerRed;

	private Queue<IGameAction> ActionQueue = new Queue<IGameAction>();


	public void Enqueue(IGameAction action)
	{
		ActionQueue.Enqueue(action);
	}

	public int GetValue(GameBoardCardQueries query)
	{
		//TODO
		return 0;
	}

	public void Process()
	{
		//TODO: Empty Action Queue
	}
	// Start is called before the first frame update
	void Start()
	{

		InitializePlayer(PlayerBlue, GameSettings.PlayerBlueCardSettings, LayerMask.GetMask("HandCardLayerBlue"));
		InitializePlayer(PlayerRed, GameSettings.PlayerRedCardSettings, LayerMask.GetMask("HandCardLayerRed"));
	}

	private void InitializePlayer(IPlayer player, PlayerCardSettingsObject settings, LayerMask mask)
	{
		string layerName = LayerMask.LayerToName(mask);
		Debug.Log($"Hand Layer({mask.value}):{layerName}");

		settings.Deck = new CardDeck(GameSettings, InitializePrefabs(settings.Cards));
		settings.Hand = new CardHand(GameSettings, settings.RotationOffset, player, settings.Deck, mask);
		player.Initialize(GameSettings, settings.Hand, settings.Deck);
	}


	private ICard[] InitializePrefabs(ICard[] cards)
	{
		ICard[] ret = new ICard[cards.Length];
		for (int i = 0; i < cards.Length; i++)
		{
			GameObject instance = Instantiate(cards[i].CardTransform.gameObject);
			ret[i] = instance.GetComponent<Card>();
		}

		return ret;
	}

}
