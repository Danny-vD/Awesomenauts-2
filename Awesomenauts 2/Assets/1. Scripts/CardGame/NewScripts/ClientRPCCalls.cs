using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class ClientRPCCalls : NetworkBehaviour
{

	public int ClientID;
	public Player Player;
	[ClientRpc]
	public void RpcDrawCards(int[] cardIds)
	{
		if (Player == null) return;
	}

	[ClientRpc]
	public void RpcBroadcastEndTurn()
	{
		if (Player == null) return;
	}

	[ClientRpc]
	public void RpcDisableInteractions(bool disable)
	{
		if (Player == null) return;
		Player.DisableInteractions = disable;
	}


}
