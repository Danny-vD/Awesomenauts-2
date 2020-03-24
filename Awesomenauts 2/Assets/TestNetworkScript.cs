using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Networking.Types;
using UnityEngine.UI;


public class TestNetworkScript : NetworkBehaviour
{


	[ClientRpc]
	public void RpcStartGame()
	{
		(NetworkManager.singleton as TestNetworkManager).SetDisplayWindow(false);
	}

	
	public void CmdUpdateOwnership()
	{
		GetComponent<NetworkIdentity>().RemoveClientAuthority();
		if (CardGameBoard.Instance.BlueTurn)
		{
			Debug.Log("Turn Ownership: " + (NetworkManager.singleton as TestNetworkManager).BluePlayer.connectionId);
			GetComponent<NetworkIdentity>()
				.AssignClientAuthority((NetworkManager.singleton as TestNetworkManager).BluePlayer);
		}
		else
		{
			
			Debug.Log("Turn Ownership: " + (NetworkManager.singleton as TestNetworkManager).RedPlayer.connectionId);
			GetComponent<NetworkIdentity>()
				.AssignClientAuthority((NetworkManager.singleton as TestNetworkManager).RedPlayer);
		}
	}

	[Command]
	public void CmdEndTurn(bool blue)
	{
		if (CardGameBoard.Instance.BlueTurn == blue)
		{
			//CardGameBoard.Instance.BlueTurn = !CardGameBoard.Instance.BlueTurn;
			RpcBroadcastEndTurn();
			CmdUpdateOwnership();
		}
	}

	public void ApplyEndTurn()
	{
		Debug.Log("AAAAAAAAAAAAAAAAAAENDTURN");

		CardGameBoard.Instance.BlueTurn = !CardGameBoard.Instance.BlueTurn;
		//PlayerBlue?.ToggleInteractions(BlueTurn);
		CardGameBoard.Instance.PlayerBlue?.ToggleInteractions(CardGameBoard.Instance.BlueTurn);
		CardGameBoard.Instance.AnimatePlayer(CardGameBoard.Instance.SocketsBlue, CardGameBoard.Instance.BlueTurn);
		CardGameBoard.Instance.PlayerRed?.ToggleInteractions(!CardGameBoard.Instance.BlueTurn);
		CardGameBoard.Instance.AnimatePlayer(CardGameBoard.Instance.SocketsRed, !CardGameBoard.Instance.BlueTurn);
	}

	[ClientRpc]
	public void RpcBroadcastEndTurn()
	{
		ApplyEndTurn();
	}


}
