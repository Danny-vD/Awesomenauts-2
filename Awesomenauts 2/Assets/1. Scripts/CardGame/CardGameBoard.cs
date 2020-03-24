using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using VDFramework.Singleton;

public class CardGameBoard : NetworkBehaviour, IGameBoard
{
	public Camera EnemyCamera;
	public float SocketAnimationSpeed;
	public float SocketAnimationIntensity;
	public float SocketAnimationYOffset;
	public float SocketCardYOffset;
	public float SocketAnimationTimingOffset;
	public CardSocket[] SocketsBlue;
	public CardSocket[] SocketsRed;
	public GameSettingsObject GameSettings;
	public CardPlayer playerBlue;
	public IPlayer PlayerBlue => playerBlue;

	private static CardGameBoard instance;
	public static CardGameBoard Instance
	{
		get
		{
			if (instance == null) throw new Exception("No Card Board");
			return instance;
		}
	}

	private bool blueTurn;
	public bool BlueTurn
	{
		get => blueTurn;
		set => blueTurn = value;
	}

	public CardPlayer playerRed;
	public IPlayer PlayerRed => playerRed;

	private Queue<IGameAction> ActionQueue = new Queue<IGameAction>();


	void Awake()
	{
		instance = this;
	}

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
		//InitializePlayer(PlayerBlue, GameSettings.PlayerBlueCardSettings);
		//InitializePlayer(PlayerRed, GameSettings.PlayerRedCardSettings);

		InitializeSockets(SocketsBlue);
		InitializeSockets(SocketsRed);

		//EndTurn(); //Begins the First Turn
	}

	public void InitializePlayer(IPlayer player, PlayerCardSettingsObject settings, NetworkConnection conn)
	{
		string layerName = LayerMask.LayerToName(UnityTrashWorkaround(settings.HandLayer));
		//Debug.Log($"Hand Layer({settings.HandLayer.value}):{layerName}");
		Card[] cards = InitializePrefabs(settings.Cards);
		settings.Deck = new CardDeck(GameSettings, cards);
		settings.Hand = new CardHand(GameSettings, settings.RotationOffset, player, settings.Deck, settings.HandLayer);
		player.Initialize(GameSettings, settings.Hand, settings.Deck);
	}

	private void InitializeSockets(CardSocket[] sockets)
	{

		for (int j = 0; j < sockets.Length; j++)
		{
			sockets[j].SetOffsetAndSpeed(j * SocketAnimationTimingOffset, SocketAnimationSpeed, SocketAnimationIntensity, SocketAnimationYOffset, SocketCardYOffset);
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



	private float ActiveTime;
	private float MaxActiveTime;


	public void AnimatePlayer(CardSocket[] sockets, bool animState)
	{

		for (int j = 0; j < sockets.Length; j++)
		{
			sockets[j].Animate(animState);
		}


	}

	public void RequestEndTurn()
	{
		Debug.Log("Turn End Request: " + isClient);
		(NetworkManager.singleton as TestNetworkManager).script.CmdEndTurn(BlueTurn);
	}

	

	private Card[] InitializePrefabs(ICard[] cards)
	{
		Card[] ret = new Card[cards.Length];
		for (int i = 0; i < cards.Length; i++)
		{
			GameObject instance = Instantiate(cards[i].CardTransform.gameObject);
			ret[i] = instance.GetComponent<Card>();
		}

		return ret;
	}

}
