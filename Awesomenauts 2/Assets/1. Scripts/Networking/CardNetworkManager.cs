using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Mirror;
using UnityEngine;

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


	public static CardNetworkManager Instance => singleton as CardNetworkManager;
	private int mapIDToLoad;
	private bool GameStarted => BoardLogic.Logic != null && BoardLogic.Logic.GameStarted;
	public bool IsHost { get; private set; }
	public bool IsServer { get; private set; }
	public MapEntry[] AvailableMaps;
	public CardEntry[] CardEntries;
	public int[] CardsInDeck = new[] { 0, 1, 0, 1, 0, 1, 0, 1, 0 };
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
		if (numPlayers == 1) StopServer();
		base.OnServerDisconnect(conn);
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

	public override void OnStartServer()
	{
		Debug.Log("Is Server");
		IsServer = true;
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
		if (!IsHost) return;
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
