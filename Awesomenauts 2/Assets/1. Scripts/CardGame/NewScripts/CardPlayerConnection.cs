using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

internal class CardPlayerConnection
{

	public NetworkConnection Connection;
	public ClientRPCCalls RPCCalls;
	public ServerCommandCalls CommandCalls;

	public CardPlayerConnection(int id, NetworkConnection conn, GameObject networkPrefab)
	{
		GameObject instance = GameObject.Instantiate(networkPrefab);
		RPCCalls = instance.GetComponent<ClientRPCCalls>();
		CommandCalls = instance.GetComponent<ServerCommandCalls>();
		Connection = conn;
		RPCCalls.ClientID = CommandCalls.ClientID = id;
	}
}
