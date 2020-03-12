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
		InitializePlayer(PlayerBlue, GameSettings.PlayerBlueCardSettings);
		InitializePlayer(PlayerRed, GameSettings.PlayerRedCardSettings);
	}

	private void InitializePlayer(IPlayer player, PlayerCardSettingsObject settings)
	{
		string layerName = LayerMask.LayerToName(UnityTrashWorkaround(settings.HandLayer));
		Debug.Log($"Hand Layer({settings.HandLayer.value}):{layerName}");

		settings.Deck = new CardDeck(GameSettings, InitializePrefabs(settings.Cards));
		settings.Hand = new CardHand(GameSettings, settings.RotationOffset, player, settings.Deck, settings.HandLayer);
		player.Initialize(GameSettings, settings.Hand, settings.Deck);
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
