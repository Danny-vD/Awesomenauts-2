using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Xml.Serialization;
using Mirror;
using Mirror.Websocket;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class CardNetworkManager : NetworkManager
{
	[Serializable]
	public struct MapEntry
	{
		public GameObject Prefab;
		public Sprite MapIcon;
	}

	[Serializable]
	public struct CardEntry
	{
		public GameObject Prefab;
		//Stats/Designs/etc
	}

	public static Dictionary<TransportType, Type> TransportTypes = new Dictionary<TransportType, Type>
	{
		{TransportType.Telepathy,  typeof(TelepathyTransport)},
		{TransportType.WebSockets, typeof(WebsocketTransport) },
	};

	public enum TransportType
	{
		Telepathy,
		WebSockets,
	}

	[Serializable]
	public class TransportInfo
	{
		public string DefaultIP = "localhost";
		public TransportType TransportType = TransportType.Telepathy;
		//public int Port = -1;
	}

	public static CardNetworkManager Instance => singleton as CardNetworkManager;
	private int mapIDToLoad;
	public TransportInfo TransportInfoData;
	public Transport[] Transports;

	public bool IsHost { get; private set; }
	public bool IsServer { get; private set; }
	private bool IsStarted;
	private bool IsStopping;
	public MapEntry[] AvailableMaps;
	public CardEntry[] CardEntries;
	public int[] CardsInDeck = new[] { 0, 1, 0, 1, 0, 1, 0, 1, 0 };

	public override void Awake()
	{
		TryLoadTransportDataFile(); //Trying to load transport info if provided
        
		base.Awake();
	}

	public void Stop()
	{
		if (!IsStarted || IsStopping) return;
		IsStopping = true;
		if (IsHost)
			StopHost();
		else if (IsServer)
			StopServer();
		else StopClient();
	}

	private void TryLoadTransportDataFile()
	{
		if (File.Exists("./transport_data.xml"))
		{
			Stream s = File.OpenRead("./transport_data.xml");
			XmlSerializer xs = new XmlSerializer(typeof(TransportInfo));
			try
			{
				TransportInfo i = (TransportInfo)xs.Deserialize(s);
				s.Close();
				TransportInfoData = i;
			}
			catch (Exception)
			{
				//Do Nothing
			}
		}
	}

	public override void OnServerSceneChanged(string sceneName)
	{
		base.OnServerSceneChanged(sceneName);

		if (sceneName == "GameScene")
		{
			MapLoader mp = FindObjectOfType<MapLoader>();
			mp.LoadMap(mapIDToLoad);
		}
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

	public override void OnClientDisconnect(NetworkConnection conn)
	{
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

	private void SetUpPlayer(int id)
	{
		CardPlayer.ServerPlayers[id].Deck.TargetSetPositions(MapTransformInfo.Instance.PlayerTransformInfos[id].DeckPosition.position, MapTransformInfo.Instance.PlayerTransformInfos[id].GravePosition.position);
		CardPlayer.ServerPlayers[id].Hand.TargetSetPosition(MapTransformInfo.Instance.PlayerTransformInfos[id].HandPosition.position);

		//Remote Call to the Client.
		CardPlayer.ServerPlayers[id].TargetSetCameraPosition(CardPlayer.ServerPlayers[id].connectionToClient, MapTransformInfo.Instance.PlayerTransformInfos[id].CameraPosition.position, MapTransformInfo.Instance.PlayerTransformInfos[id].CameraPosition.rotation, id == 1);
	}

	public override void OnServerReady(NetworkConnection conn)
	{
		Debug.Log("Client Ready: " + conn.connectionId);
		base.OnServerReady(conn);

	}

	private void Update()
	{
		if (!IsStarted && SystemInfo.graphicsDeviceType == GraphicsDeviceType.Null) //not started and Headless
		{
			if (SceneManager.GetActiveScene().name == "MenuScene")
			{
				Debug.Log("Starting Server...");
				StartServer();
			}
			return;
		}


		if (!IsServer) return;

		if (CardPlayer.ServerPlayers.Count == 2 && CardPlayer.AllPlayersReady && !BoardLogic.Logic.GameStarted)
		{
			//Thread.Sleep(1000); //Hack: Wait for client to create the player after connection established.
			SetUpPlayer(0);
			SetUpPlayer(1);
			for (int i = 0; i < CardPlayer.ServerPlayers.Count; i++)
			{
				CardPlayer.ServerPlayers[i].DrawCard(5);
			}

			BoardLogic.Logic.StartGame();


			//Each Player Draws Cards
			//  Need
		}
	}
}
