using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class BoardLogic : NetworkBehaviour
{
	public bool GameStarted;

	private int CurrentTurn = -1;
	public int CurrentTurnClient { get; private set; }

	public static BoardLogic Logic;

	// Start is called before the first frame update
	void Start()
	{
		Logic = this;
	}

	public void DisconnectNetwork()
	{
		CardNetworkManager.Instance.Stop();
	}

	[Client]
	public void ClientRequestEndTurn()
	{
		CardPlayer.LocalPlayer.EndTurn();
	}

	[Server]
	public void StartGame()
	{
		GameStarted = true;
		int[] clientIDs = new int[CardPlayer.ServerPlayers.Count];
		int[] teamIDs = new int[CardPlayer.ServerPlayers.Count];
		for (int i = 0; i < CardPlayer.ServerPlayers.Count; i++)
		{
			clientIDs[i] = CardPlayer.ServerPlayers[i].ClientID;
			teamIDs[i] = i;
		}

		MapTransformInfo.Instance.SocketManager.AddPlayers(clientIDs, teamIDs);
		RpcBroadcastStartGame(clientIDs, teamIDs);
		ServerEndTurn();
	}

	[ClientRpc]
	public void RpcBroadcastStartGame(int[] clientIds, int[] teamIDs)
	{
		MapTransformInfo.Instance.SocketManager.AddPlayers(clientIds, teamIDs);
	}

	[Server]
	public void ServerEndTurn()
	{
		CurrentTurn++;
		if (CurrentTurn >= CardPlayer.ServerPlayers.Count)
		{
			CurrentTurn = 0;
		}

		CardPlayer current = CardPlayer.ServerPlayers[CurrentTurn];
		CurrentTurnClient = current.ClientID;

		Debug.Log("Next Turn Client ID: " + CurrentTurnClient + "\nTurnNumber: " + CurrentTurn);
		if (CardPlayer.LocalPlayer != null)
			CardPlayer.LocalPlayer.EnableInteractions = CardPlayer.LocalPlayer.ClientID == CurrentTurnClient;
		MapTransformInfo.Instance.SocketManager.SetTurn(CurrentTurnClient);
		RpcBroadcastEndTurn(CurrentTurnClient);

		current.DrawCard(1); //Next player is drawing one card

	}

	[ClientRpc]
	public void RpcBroadcastEndTurn(int next)
	{
		if (CardNetworkManager.Instance.IsHost) return; //Has Already been set by ServerEndTurn if we are playing and hosting at the same time


		CurrentTurn++;

		Debug.Log("Client Received End Turn\nOld: " + CurrentTurnClient + "\nNew: " + next);
		CurrentTurnClient = next;
		CardPlayer.LocalPlayer.EnableInteractions = CardPlayer.LocalPlayer.ClientID == next;

		MapTransformInfo.Instance.SocketManager.SetTurn(CurrentTurnClient);
	}

}
