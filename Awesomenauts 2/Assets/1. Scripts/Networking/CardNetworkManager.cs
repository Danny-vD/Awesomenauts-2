using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Assets._1._Scripts.ScriptableObjects.Effects;
using DataObjects;
using Events.Deckbuilder;
using Maps;
using Player;
using Utility;
using Mirror;
using Mirror.Websocket;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using VDFramework.EventSystem;
using Logger = MasterServer.Common.Logger;


namespace Networking
{
	public class CardNetworkManager : NetworkManager
	{

		public class ExitFlag
		{
			public string message;
		}

		public static ExitFlag Exit;

		public Sprite DefaultCardPortrait;
		public BorderInfo DefaultCardBorder;

		public static CardNetworkManager Instance => singleton as CardNetworkManager;
		public bool IsHost { get; private set; }
		public bool IsServer { get; private set; }

		public EndPointInfo CurrentEndPoint
		{
			get => currentEndPoint;
			set
			{
				currentEndPoint = value;
				if (!CanApplyEndPoint)
				{
					Debug.LogWarning("Can not change End Point info at the moment.");
				}
				else
				{
					ApplyEndPoint();
				}
			}
		}

		private readonly bool CanApplyEndPoint = true;
		private EndPointInfo currentEndPoint;

		private float TimeStamp;
		private int mapIDToLoad;
		private bool IsStarted;
		private bool IsStopping;


		[Header("Card Networking Parameters")]
		public MapEntry[] AvailableMaps;

		public int StartingCards = 4;
		public CardEntry CompensationCard;
		public GameObject BoardLogicPrefab;
		public CardEntry[] CardEntries;
		public int[] CardsInDeck { get; private set; } = new int[0];


		public Sprite GetCardImage(string name)
		{
			foreach (CardEntry cardEntry in CardEntries)
			{
				if (cardEntry.Statistics.GetValue<string>(CardPlayerStatType.CardName) == name)
				{
					return cardEntry.Sprites.Portrait;
				}
			}

			return DefaultCardPortrait;
		}
		public BorderInfo GetCardBorder(string name)
		{
			foreach (CardEntry cardEntry in CardEntries)
			{

				if (cardEntry.Statistics.GetValue<string>(CardPlayerStatType.CardName) == name && cardEntry.cardBorder.IsValid)
				{
					return cardEntry.cardBorder;
				}
			}

			return DefaultCardBorder;
		}

		public List<AEffect> GetCardEffects(string name)
		{
			foreach (CardEntry cardEntry in CardEntries)
			{
				if (cardEntry.Statistics.GetValue<string>(CardPlayerStatType.CardName) == name)
				{
					return cardEntry.effects;
				}
			}

			return new List<AEffect>();
		}


		public override void Awake()
		{
			if (File.Exists("./DeckConfig.xml"))
			{
				Stream s = File.OpenRead("./DeckConfig.xml");
				XmlSerializer xs = new XmlSerializer(typeof(DeckConfig));
				CardsInDeck = ((DeckConfig)xs.Deserialize(s)).CardIDs;
				s.Close();
			}

			Application.quitting += Application_quitting;

			EventManager.Instance.AddListener<SaveDeckEvent>(SaveDeckEventHandler);
			if (SystemInfo.graphicsDeviceType == GraphicsDeviceType.Null) IsServer = true;
			Logger.DefaultLogger = Debug.Log;
			base.Awake();

		}

		public struct DeckConfig
		{
			public int[] CardIDs;
		}


		private void Application_quitting()
		{
			Stream s = File.Create("./DeckConfig.xml");
			XmlSerializer xs = new XmlSerializer(typeof(DeckConfig));
			xs.Serialize(s, new DeckConfig() { CardIDs = CardsInDeck });
			s.Close();
		}

		private void SaveDeckEventHandler(SaveDeckEvent obj)
		{
			SetCardsInDeck(obj.CardIDs);
		}

		public override void Start()
		{
			CompensationCard.Statistics.InitializeStatDictionary();

			foreach (CardEntry cardEntry in CardEntries)
			{
				cardEntry.Statistics.InitializeStatDictionary();
			}

			CurrentEndPoint = GameInitializer.Data.Network.DefaultAddress;
			//RefreshEndPoint();
			base.Start();

			TimeStamp = Time.realtimeSinceStartup;
		}

		public void LoadMap(int id)
		{
			Debug.Log("Loading Map: " + id);
			GameObject map = Instantiate(AvailableMaps[id].Prefab);
			NetworkServer.Spawn(map);
		}


		private void Update()
		{
			//if (initDelay > 0)
			//{
			//	initDelay -= Time.deltaTime;
			//	return;
			//}

			if (IsStarted && SystemInfo.graphicsDeviceType == GraphicsDeviceType.Null)
			{
				if (GameInitializer.Data.HeadlessInfo.NoPlayerTimeout != -1 && numPlayers == 0 && TimeStamp + GameInitializer.Data.HeadlessInfo.NoPlayerTimeoutSeconds < Time.realtimeSinceStartup || //Timeout because of no players
					GameInitializer.Data.HeadlessInfo.Timeout != -1 && TimeStamp + GameInitializer.Data.HeadlessInfo.TimeoutSeconds < Time.realtimeSinceStartup) //Timeout because of time limit
				{
					Application.Quit();
				}
			}



			if (!IsStarted && SystemInfo.graphicsDeviceType == GraphicsDeviceType.Null) //not started and Headless
			{
				if (SceneManager.GetActiveScene().name == "MenuScene")
				{
					Debug.Log("Starting Server...");
					StartServer();
				}
				return;
			}


			if (!IsServer || BoardLogic.Logic == null) return;

			if (CardPlayer.ServerPlayers.Count == 2 && CardPlayer.AllPlayersReady && !BoardLogic.Logic.GameStarted)
			{

				//Thread.Sleep(1000); //Hack: Wait for client to create the player after connection established.
				SetUpPlayer(0);
				SetUpPlayer(1);
				for (int i = 0; i < CardPlayer.ServerPlayers.Count; i++)
				{
					if (i != 0)
					{
						CardPlayer.ServerPlayers[i].DrawCard(StartingCards);
						CardPlayer.ServerPlayers[i].DrawCard(CompensationCard);
					}
					else
					{
						CardPlayer.ServerPlayers[i].DrawCard(StartingCards - 1);
					}
				}

				BoardLogic.Logic.StartGame();


				//Each Player Draws Cards
				//  Need
			}
		}


		#region Private Functions

		private void SetUpPlayer(int id)
		{
			CardPlayer.ServerPlayers[id].Deck.TargetSetPositions(MapTransformInfo.Instance.PlayerTransformInfos[id].DeckPosition.position, MapTransformInfo.Instance.PlayerTransformInfos[id].GravePosition.position);
			CardPlayer.ServerPlayers[id].Hand.TargetSetPosition(MapTransformInfo.Instance.PlayerTransformInfos[id].HandPosition.position);

			//Remote Call to the Client.
			CardPlayer.ServerPlayers[id].TargetSetCameraPosition(CardPlayer.ServerPlayers[id].connectionToClient, MapTransformInfo.Instance.PlayerTransformInfos[id].CameraPosition.position, MapTransformInfo.Instance.PlayerTransformInfos[id].CameraPosition.rotation, id == 1);
		}

		#endregion

		#region Overrides


		public override void OnServerSceneChanged(string sceneName)
		{
			base.OnServerSceneChanged(sceneName);

			if (sceneName == "GameScene")
			{
				GameObject blogic = Instantiate(BoardLogicPrefab);
				NetworkServer.Spawn(blogic);
				LoadMap(mapIDToLoad);
			}
		}

		public override void OnClientConnect(NetworkConnection conn)
		{
			//GameInitializer.Master.SetConnectionSuccess(); //Only needs to be called if MasterServer API is in ReconnectLoop.
			base.OnClientConnect(conn);
		}


		public override void OnServerConnect(NetworkConnection conn)
		{
			base.OnServerConnect(conn);
		}

		public override void OnClientDisconnect(NetworkConnection conn)
		{
			//GameInitializer.Master.SetConnectionSuccess(); //Only needs to be called if MasterServer API is in ReconnectLoop.
			Debug.Log("Disconnected From Server");
			CleanUp();
			base.OnClientDisconnect(conn);
		}

		public override void OnServerDisconnect(NetworkConnection conn)
		{
			Debug.Log("Client Disconnected. Remaining: " + numPlayers);
			if (numPlayers == 1 && !IsStopping)
				Stop();
			base.OnServerDisconnect(conn);
		}

		public override void OnStopClient()
		{
			if (!IsServer && !IsHost)
			{
				CleanUp();
			}
			base.OnStopClient();
		}

		public override void OnStopServer()
		{
			Debug.Log("Server Stopped.");
			CleanUp();
			base.OnStopServer();
		}

		public override void OnStartHost()
		{
			Debug.Log("Is Host");
			IsHost = true;
			base.OnStartHost();


		}

		public override void OnStartClient()
		{
			StartNetwork();
			base.OnStartClient();
		}

		public override void OnStartServer()
		{
			StartNetwork();
			IsServer = true;
			Debug.Log("Is Server");
			base.OnStartServer();
		}



		public override void OnServerReady(NetworkConnection conn)
		{
			Debug.Log("Client Ready: " + conn.connectionId);
			base.OnServerReady(conn);

		}

		#endregion

		#region Public Functions

		/// <summary>
		/// Hack to change the Card Deck based on wether you join or host a game.
		/// On Headless servers this will result in both clients having the "joingame" deck
		/// </summary>
		/// <param name="id"></param>
		public void SetCardsInDeck(int[] id) //CardNetworkManager
		{
			//Getting the ID of the Card:
			//CardNetworkManager has Array of card entries. All cards that are in the game need to exist in this array.
			//This array defines the index of the card.
			//One way to go about it is to store the deck builder info(card image/etc..) in the card entry.
			//When loading the deck builder you can iterate over the card entries and get their info/index.
			//When adding a card you just remove or add the cards index to a list of int.
			//Your Set Cards In Deck Array would just be the list.ToArray().

			CardsInDeck = id; //Hack. You have to pass the int array with indices to this function instead of this one int.
		}

		public void ApplyEndPoint()
		{
			if (CurrentEndPoint == null || CurrentEndPoint.Port <= 0 || CurrentEndPoint.Port >= ushort.MaxValue) return;

			Debug.Log("New End Point: " + CurrentEndPoint);

			networkAddress = CurrentEndPoint.IP;
			(transport as WebsocketTransport).port = CurrentEndPoint.Port;
		}

		/// <summary>
		/// Stops the NetworkManager in whatever he is doing
		/// Either Hosting/ServerOnly or Client
		/// </summary>
		public void Stop()
		{
			if (!IsStarted || IsStopping) return;
			IsStopping = true;
			if (IsHost)
				StopHost();
			else if (IsServer)
				StopServer();
			else StopClient();


			if (GameInitializer.Data.HeadlessInfo.CloseOnMatchEnded) Application.Quit();
		}



		private void CleanUp()
		{
			Debug.Log("Cleaning Up...");
			CardPlayer.LocalPlayer = null;
			CardPlayer.ServerPlayers.Clear();
			BoardLogic.Logic = null;
			IsServer = false;
			IsHost = false;
			IsStarted = false;

		}

		private void StartNetwork()
		{
			IsStarted = true;
			IsStopping = false;
		}

		public void SetMapToLoad(int id)
		{
			mapIDToLoad = id;
		}


		#endregion




	}
}
