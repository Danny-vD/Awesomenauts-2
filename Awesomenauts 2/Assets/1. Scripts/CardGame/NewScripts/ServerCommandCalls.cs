using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class ServerCommandCalls : NetworkBehaviour
{
	public int ClientID;
	public Player Player;
	[Command]
	public void CmdRequestEndTurn()
	{
		Board.Instance.EndTurn(ClientID);
	}

	[Command]
	public void CmdRequestPlayCard(int cardID)
	{

	}
}
