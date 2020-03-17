using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGameBoard : MonoBehaviour, IGameBoard
{
	public float SocketAnimationSpeed;
	public float SocketAnimationIntensity;
	public float SocketAnimationOffset;
	public CardSocket[] SocketsBlue;
	public CardSocket[] SocketsRed;
	public GameSettingsObject GameSettings;
	public CardPlayer playerBlue;
	public IPlayer PlayerBlue => playerBlue;

	public bool BlueTurn { get; set; }

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

		InitializeSockets(SocketsBlue);
		InitializeSockets(SocketsRed);

		EndTurn(); //Begins the First Turn
	}

	private void InitializePlayer(IPlayer player, PlayerCardSettingsObject settings)
	{
		string layerName = LayerMask.LayerToName(UnityTrashWorkaround(settings.HandLayer));
		//Debug.Log($"Hand Layer({settings.HandLayer.value}):{layerName}");

		settings.Deck = new CardDeck(GameSettings, InitializePrefabs(settings.Cards));
		settings.Hand = new CardHand(GameSettings, settings.RotationOffset, player, settings.Deck, settings.HandLayer);
		player.Initialize(GameSettings, settings.Hand, settings.Deck);
	}

	private void InitializeSockets(CardSocket[] sockets)
	{

		for (int j = 0; j < sockets.Length; j++)
		{
			sockets[j].SetOffsetAndSpeed(j * SocketAnimationOffset, SocketAnimationSpeed, SocketAnimationIntensity);
		}

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


	public void EndTurn()
	{
		BlueTurn = !BlueTurn;

		Debug.Log($"Active Player: {(BlueTurn ? "Blue" : "Red")}");

		PlayerBlue.ToggleInteractions(BlueTurn);
		AnimatePlayer(SocketsBlue, BlueTurn);
		PlayerRed.ToggleInteractions(!BlueTurn);
		AnimatePlayer(SocketsRed, !BlueTurn);
	}

	private float ActiveTime;
	private float MaxActiveTime;

	private void AnimatePlayer(CardSocket[] sockets, bool animState)
	{

		for (int j = 0; j < sockets.Length; j++)
		{
			sockets[j].Animate(animState);
		}


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
