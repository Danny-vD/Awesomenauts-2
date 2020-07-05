using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Assets._1._Scripts.ScriptableObjects.Effects;
using DataObjects;
using Events.Deckbuilder;
using Maps;
using Player;
using Utility;
using Mirror;
using Mirror.Websocket;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using VDFramework.EventSystem;
using VDFramework.Extensions;
using Logger = MasterServer.Common.Logger;


namespace Networking
{
	public class CardNetworkManager : NetworkManager
	{


		private static string DataPath => Application.platform == RuntimePlatform.Android
			? Application.persistentDataPath + "/" : "./";

		public static string DeckPath => DataPath + "DeckConfig.xml";
		public static string ErrorPath => DataPath + "CrashLogs/";
		public static string GetErrorFile(string key) => ErrorPath + DateTime.Now.ToString("yy-MM-dd-hh-mm-ss") + "-" + DateTime.Now.Millisecond + "_" + key + ".log";


		public class ExitFlag
		{
			public string message;
		}

		public static ExitFlag Exit;

		public List<AEffect> AllEffects;

		public Sprite DefaultCardPortrait;
		public TeamBorderInfo DefaultCardBorder;

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
		public GameObject BoardLogicPrefab;
		public CardEntry[] CardEntries;
		public int[] CardsInDeck { get; private set; } = new int[0];

		public CardEntry Turret => GetEntry("Turret");

		//      [Serializable]
		//public class TeamPrefabAsset : TeamAsset<TeamPrefabObject> { }
		//public TeamPrefabAsset TeamPrefabs;

		public CardEntry GetAwsomenaut(int teamID)
		{
			return teamID == 1 ? GetEntry("Voltar") : GetEntry("Lonestar");
		}



		public Sprite GetCardImage(string name, int teamID)
		{
			CardEntry e = GetEntry(name);
			if (e.Sprites.TeamPortrait != null)
				return e.Sprites.TeamPortrait.Get(teamID) != null ? e.Sprites.TeamPortrait.Get(teamID) : DefaultCardPortrait;
			return DefaultCardPortrait;
		}


		public GameObject GetCardModel(string name, int teamID)
		{
			CardEntry e = GetEntry(name);
			return e.Model?.Get(teamID);
		}

		public CardEntry GetEntry(string name)
		{
			foreach (CardEntry cardEntry in CardEntries)
			{
				if (cardEntry.Statistics.GetValue<string>(CardPlayerStatType.CardName) == name)
				{
					return cardEntry;
				}
			}
			ExceptionViewUI.Instance.SetException(new UnityException("Can not find card with Name: " + name));
			return new CardEntry();
		}

		public BorderInfo GetCardBorder(string name, int teamID)
		{
			CardEntry e = GetEntry(name);
			if (e.cardBorder != null)
				return e.cardBorder.IsValid ? e.cardBorder : DefaultCardBorder.Get(teamID);
			return DefaultCardBorder.Get(teamID);
		}

		public List<AEffect> GetCardEffects(string name)
		{
			return GetEntry(name).effects;
		}


		public override void Awake()
		{
			for (int i = 0; i < CardEntries.Length; i++)
			{
				CardEntry cardEntry = CardEntries[i];

				cardEntry.index = i;

				CardEntries[i] = cardEntry;
			}

			if (File.Exists(DeckPath))
			{
				Stream s = File.OpenRead(DeckPath);
				XmlSerializer xs = new XmlSerializer(typeof(DeckConfig));
				SetCardsInDeck(((DeckConfig)xs.Deserialize(s)).CardIDs);
				s.Close();
				ShowDataPathScript.Write("Loaded: " + CardsInDeck.Length + " Entries");
			}

			Application.quitting += Application_quitting;

			EventManager.Instance.AddListener<SaveDeckEvent>(SaveDeckEventHandler);
			EventManager.Instance.AddListener<ValidDeckEvent>(OnValidDeckEvent);
			if (SystemInfo.graphicsDeviceType == GraphicsDeviceType.Null) IsServer = true;
			Logger.DefaultLogger = Debug.Log;
			base.Awake();

		}

		private void OnValidDeckEvent(ValidDeckEvent obj)
		{
			ShowDataPathScript.Write("Deck Valid: " + obj.IsValid);
		}

		public struct DeckConfig
		{
			public int[] CardIDs;
		}


		private void Application_quitting()
		{
			MasterServerComponent ms = GetComponent<MasterServerComponent>();
			ms.AbortQueue();

			ShowDataPathScript.Write("Saved: " + CardsInDeck.Length + " Entries");
			Stream s = File.Create(DeckPath);
			XmlSerializer xs = new XmlSerializer(typeof(DeckConfig));
			xs.Serialize(s, new DeckConfig() { CardIDs = CardsInDeck });
			s.Close();
		}

		private void SaveDeckEventHandler(SaveDeckEvent obj)
		{
			ShowDataPathScript.Write("Saved: " + obj.CardIDs.Length + " Entries");
			SetCardsInDeck(obj.CardIDs);
			Stream s = File.Create(DeckPath);
			XmlSerializer xs = new XmlSerializer(typeof(DeckConfig));
			xs.Serialize(s, new DeckConfig() { CardIDs = CardsInDeck });
			s.Close();
		}

		public override void Start()
		{

			foreach (CardEntry cardEntry in CardEntries)
			{
				cardEntry.Statistics.InitializeStatDictionary();
			}

			CurrentEndPoint = GameInitializer.Data.Network.DefaultAddress;
			//RefreshEndPoint();
			base.Start();

			TimeStamp = Time.realtimeSinceStartup;

			Transport.activeTransport.OnClientError.RemoveAllListeners();
			Transport.activeTransport.OnClientError.AddListener(OnClientErr);
			Transport.activeTransport.OnServerError.RemoveAllListeners();
			Transport.activeTransport.OnServerError.AddListener(OnServerErr);

		}

		private void OnServerErr(int arg0, Exception arg1)
		{
			ExceptionViewUI.Instance.SetException(arg1, $"Host Error({arg0}):");
			Stop();
			CleanUp();
		}

		private void OnClientErr(Exception arg0)
		{
			ExceptionViewUI.Instance.SetException(arg0, "Client Error:");
			Stop();
			CleanUp();
		}

		public void LoadMap(int id)
		{
			//Debug.Log("Loading Map: " + id);
			ReplaceWithNetworkObject.ApplyConnections = null;
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
					//Debug.Log("Starting Server...");
					StartServer();
				}
				return;
			}


			if (!IsServer || BoardLogic.Logic == null) return;

			if (numPlayers == 2 && CardPlayer.ServerPlayers.Count == 2 && BoardLogic.Logic.CanStartGame)
			{
				Debug.Log("Setting Up Players.");
				//Thread.Sleep(1000); //Hack: Wait for client to create the player after connection established.
				SetUpPlayer(0);
				SetUpPlayer(1);

				Debug.Log("Applying Socket Node Connections.");
				ReplaceWithNetworkObject.ApplyConnections();


				BoardLogic.Logic.StartGame();


				//Each Player Draws Cards
				//  Need
			}
		}


		#region Private Functions

		private void SetUpPlayer(int id)
		{
			CardPlayer.ServerPlayers[id].Deck.RpcSetPositions(MapTransformInfo.Instance.PlayerTransformInfos[id].DeckPosition.position, MapTransformInfo.Instance.PlayerTransformInfos[id].GravePosition.position);
			CardPlayer.ServerPlayers[id].Deck.SetPositions(MapTransformInfo.Instance.PlayerTransformInfos[id].DeckPosition.position, MapTransformInfo.Instance.PlayerTransformInfos[id].GravePosition.position);
			CardPlayer.ServerPlayers[id].Hand.SetPosition(MapTransformInfo.Instance.PlayerTransformInfos[id].HandPosition.position);
			CardPlayer.ServerPlayers[id].Hand.RpcSetPosition(MapTransformInfo.Instance.PlayerTransformInfos[id].HandPosition.position);
			CardPlayer.ServerPlayers[id].Hand.SetCameraPosition(MapTransformInfo.Instance.PlayerTransformInfos[id].CameraPosition.position);
			CardPlayer.ServerPlayers[id].Hand.RpcSetCameraPosition(MapTransformInfo.Instance.PlayerTransformInfos[id].CameraPosition.position);

			//Remote Call to the Client.
			CardPlayer.ServerPlayers[id].RpcSetCameraPosition(MapTransformInfo.Instance.PlayerTransformInfos[id].CameraPosition.position, MapTransformInfo.Instance.PlayerTransformInfos[id].CameraPosition.rotation, id == 1);

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
			//Debug.Log("Disconnected From Server");
			CleanUp();
			base.OnClientDisconnect(conn);
		}

		public override void OnServerDisconnect(NetworkConnection conn)
		{
			//Debug.Log("Client Disconnected. Remaining: " + numPlayers);
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
			//Debug.Log("Server Stopped.");
			CleanUp();
			base.OnStopServer();
		}

		public override void OnStartHost()
		{
			//Debug.Log("Is Host");
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
			//Debug.Log("Is Server");
			base.OnStartServer();
		}



		public override void OnServerReady(NetworkConnection conn)
		{
			//Debug.Log("Client Ready: " + conn.connectionId);
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


			CardsInDeck = !GameInitializer.Instance.GameData.DebugInfo.NoShuffleDecks ? id.Randomize().ToArray() : id;
		}

		public void ApplyEndPoint()
		{
			if (CurrentEndPoint == null || CurrentEndPoint.Port <= 0 || CurrentEndPoint.Port >= ushort.MaxValue) return;

			//Debug.Log("New End Point: " + CurrentEndPoint);

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
			//Debug.Log("Cleaning Up...");
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
