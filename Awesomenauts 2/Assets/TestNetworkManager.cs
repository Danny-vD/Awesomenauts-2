using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class TestNetworkManager : NetworkManager
{
	public GameObject cardPlayerPrefab;
	public GameObject cardEnemyPrefab;
	public NetworkConnection RedPlayer;
	public NetworkConnection BluePlayer;
	public TestNetworkScript script;
	public override void OnClientConnect(NetworkConnection conn)
	{
		if (conn.connectionId == 0)
		{
			playerPrefab = cardPlayerPrefab;
		}
		else playerPrefab = cardEnemyPrefab;
		base.OnClientConnect(conn);
	}

	public override void OnServerConnect(NetworkConnection conn)
	{
		Debug.Log("Server Connection ID: " + conn.connectionId);
		if (conn.connectionId == 0)
		{
			RedPlayer = conn;
			playerPrefab = cardPlayerPrefab;
		}
		else
		{
			BluePlayer = conn;
			playerPrefab = cardEnemyPrefab;
		}
		base.OnServerConnect(conn);
	}


	public Text IPField;
	public GameObject WaitingWindow;


	public void StartHostSide()
	{
		networkAddress = IPField.text == "" ? "localhost" : IPField.text;
		StartHost();

		SetDisplayWindow(true);
		
	}

	public void StartClientSide()
	{
		networkAddress = IPField.text == "" ? "localhost" : IPField.text;
		StartClient();

		SetDisplayWindow(true);
	}

	public void SetDisplayWindow(bool active)
	{
		if (active != WaitingWindow.activeSelf)
			WaitingWindow.SetActive(active);
	}



	private bool init;
	void Update()
	{
		if (numPlayers == 2 && !init)
		{
			init = true;
			script.RpcStartGame();

			script.CmdUpdateOwnership();
			script.RpcBroadcastEndTurn(); //Begins the First Turn
			
		}
	}
}
