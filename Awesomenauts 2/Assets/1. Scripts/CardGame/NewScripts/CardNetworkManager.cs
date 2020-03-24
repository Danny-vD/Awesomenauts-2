using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CardNetworkManager : NetworkManager
{
	public string MenuScene;
	public static CardNetworkManager Instance { get; private set; }
	private List<CardPlayerConnection> Connections = new List<CardPlayerConnection>();
	public GameObject NetworkCallsPrefab;
	public int ConnectionCount => Connections.Count;

	private CardNetworkManager()
	{
		Instance = this;
	}

	public void RegisterPlayer(Player player)
	{
		CardPlayerConnection cpc= Connections.Find(x => x.RPCCalls.ClientID == player.AssociatedClient);
		cpc.RPCCalls.Player = cpc.CommandCalls.Player = player;
	}

	internal CardPlayerConnection GetConnection(int index)
	{
		if (index < 0 || index >= ConnectionCount) throw new IndexOutOfRangeException("Index: " + index + " is not valid as an index for the Network Connections.");
		return Connections[index];
	}

	public override void OnServerAddPlayer(NetworkConnection conn)
	{
		CardPlayerConnection cpc = new CardPlayerConnection(ConnectionCount, conn, NetworkCallsPrefab);
		playerPrefab.GetComponent<Player>().AssociatedClient = ConnectionCount;
		Connections.Add(cpc);
		base.OnServerAddPlayer(conn);
        
	}

	public override void OnServerDisconnect(NetworkConnection conn)
	{
		Connections.RemoveAt(Connections.FindIndex(x=>x.Connection==conn));
		base.OnServerDisconnect(conn);
	}

	public override void OnClientDisconnect(NetworkConnection conn)
	{
		base.OnClientDisconnect(conn);
		SceneManager.LoadScene(MenuScene, LoadSceneMode.Single);
	}
}
