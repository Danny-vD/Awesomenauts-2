using System;
using System.Collections.Generic;
using System.Linq;
using Assets._1._Scripts.ScriptableObjects.Effects;
using Networking;
using Player;
using Utility;
using Mirror;
using UI.DebugPanel;
using UnityEngine;
using Random = UnityEngine.Random;
using Version = System.Version;

namespace Maps
{
	public class BoardLogic : NetworkBehaviour
	{
		public bool GameStarted;

		private int MaxSolar = 1;
		private int CurrentTurn = -1;
		public int CurrentTurnClient { get; private set; }

		private float TimeStamp;
		public static BoardLogic Logic;
		public Action OnEndTurn;
		public Action OnStartTurn;

		// Start is called before the first frame update
		private void Awake()
		{
			Logic = this;
			TimeStamp = Time.realtimeSinceStartup + Random.Range(15f, 30f);
		}

		private void Update()
		{
			if (GameInitializer.Data.DebugInfo.DebugServerQuit && Time.realtimeSinceStartup > TimeStamp)
			{
				CardNetworkManager.Instance.Stop();
			}
		}

		#region Client Remote Procedure Calls

		//Gets called on all Clients to start the Game.
		//This Will map the clients to the teams on the board.
		[ClientRpc]
		public void RpcBroadcastStartGame(int[] clientIds, int[] teamIDs)
		{
			MapTransformInfo.Instance.SocketManager.AddPlayers(clientIds, teamIDs);
		}

		//Gets Called on all Clients to end the current turn
		//next is the next players ID that has the turn.
		[ClientRpc]
		public void RpcBroadcastEndTurn()
		{
			if (CardNetworkManager.Instance.IsHost) return;

			if (CurrentTurn != -1)
			{
				MapTransformInfo.Instance.SocketManager.TriggerEffect(EffectTrigger.AfterRoundEnd,
					CardPlayer.ServerPlayers[CurrentTurn]);
			}
			CurrentTurn++;
			if (MaxSolar >= 10) MaxSolar = 10;
			if (CurrentTurn >= CardPlayer.ServerPlayers.Count)
			{
				CurrentTurn = 0;
				MaxSolar++;
			}

			CardPlayer current = CardPlayer.ServerPlayers[CurrentTurn];
			current.ClearMovedCards();
			CurrentTurnClient = current.ClientID;

			//Debug.Log("Next Turn Client ID: " + CurrentTurnClient + "\nTurnNumber: " + CurrentTurn);
			if (CardPlayer.LocalPlayer != null)
			{
				CardPlayer.LocalPlayer.EnableInteractions = CardPlayer.LocalPlayer.ClientID == CurrentTurnClient;
				if (CardPlayer.LocalPlayer.ClientID == CurrentTurnClient)
				{
					OnStartTurn?.Invoke();
				}
				else
				{
					OnEndTurn?.Invoke();
				}
			}
			MapTransformInfo.Instance.SocketManager.SetTurn(CurrentTurnClient);

			//current.DrawCard(1); //Next player is drawing one card
			current.PlayerStatistics.SetValue(CardPlayerStatType.Solar, MaxSolar);
			//if (CardNetworkManager.Instance.IsHost) return; //Has Already been set by ServerEndTurn if we are playing and hosting at the same time


			MapTransformInfo.Instance.SocketManager.TriggerEffect(EffectTrigger.OnRoundStart, current);


			//CurrentTurn++;

			////Debug.Log("Client Received End Turn\nOld: " + CurrentTurnClient + "\nNew: " + next);
			//CurrentTurnClient = next;
			//CardPlayer.LocalPlayer.EnableInteractions = CardPlayer.LocalPlayer.ClientID == next;

			//MapTransformInfo.Instance.SocketManager.SetTurn(CurrentTurnClient);
		}

		#endregion

		#region Server Only Calls

		//Starts the Game
		//Sets up the client ids and the teamids
		[Server]
		public void StartGame()
		{
			RpcSetServerVersion(Application.version);
			GameStarted = true;
			int[] clientIDs = new int[CardPlayer.ServerPlayers.Count];
			int[] teamIDs = new int[CardPlayer.ServerPlayers.Count];
			for (int i = 0; i < CardPlayer.ServerPlayers.Count; i++)
			{
				clientIDs[i] = CardPlayer.ServerPlayers[i].ClientID;
				teamIDs[i] = i;
			}


			MapTransformInfo.Instance.SocketManager.AddPlayers(clientIDs, teamIDs);
			//Debug.Log("Player Count: "+ CardPlayer.ServerPlayers.Count);


			for (int i = 0; i < CardPlayer.ServerPlayers.Count; i++)
			{


				Card turretLeft = CardPlayer.ServerPlayers[i].CreateCard(CardNetworkManager.Instance.TeamPrefabs[i].TurretLeft);
				Card turretRight = CardPlayer.ServerPlayers[i].CreateCard(CardNetworkManager.Instance.TeamPrefabs[i].TurretRight);
				Card awsomenaut = CardPlayer.ServerPlayers[i].CreateCard(CardNetworkManager.Instance.TeamPrefabs[i].Awsomenaut);
				//Find Ally Sockets for turrets
				List<CardSocket> socks = new List<CardSocket>();
				MapTransformInfo.Instance.SocketManager.CardSockets.Select(x => x.CardSockets).ToList()
					.ForEach(x => socks.AddRange(x));
				CardSocket ltS = socks.First(x => MapTransformInfo.Instance.SocketManager.IsFromTeam(CardPlayer.ServerPlayers[i].ClientID, x.transform) &&
												  x.SocketType == SocketType.TurretLeft);
				CardSocket rtS = socks.First(x => MapTransformInfo.Instance.SocketManager.IsFromTeam(CardPlayer.ServerPlayers[i].ClientID, x.transform) &&
												  x.SocketType == SocketType.TurretRight);
				CardSocket awS = socks.First(x => MapTransformInfo.Instance.SocketManager.IsFromTeam(CardPlayer.ServerPlayers[i].ClientID, x.transform) &&
				                                  x.SocketType == SocketType.Awsomenaut);
				ltS.RpcDockCard(turretLeft.netIdentity);
				rtS.RpcDockCard(turretRight.netIdentity);
				awS.RpcDockCard(awsomenaut.netIdentity);
				//Set Turrets/Awsomenauts for all clients
			}

			RpcBroadcastStartGame(clientIDs, teamIDs);
			ServerEndTurn();
		}
		[ClientRpc]
		private void RpcSetServerVersion(string version)
		{

			//if (Version.Parse(version) < Version.Parse(Application.version))
			//{

			//}

			DebugPanelInfo.instance.UpdateVersionText(version);
		}

		//Ends the turn
		[Server]
		public void ServerEndTurn()
		{
			if (!GameStarted) return;

			if (CurrentTurn != -1)
			{
				MapTransformInfo.Instance.SocketManager.TriggerEffect(EffectTrigger.AfterRoundEnd,
					CardPlayer.ServerPlayers[CurrentTurn]);
			}
			CurrentTurn++;
			if (MaxSolar >= 10) MaxSolar = 10;
			if (CurrentTurn >= CardPlayer.ServerPlayers.Count)
			{
				CurrentTurn = 0;
				MaxSolar++;
			}

			CardPlayer current = CardPlayer.ServerPlayers[CurrentTurn];
			current.ClearMovedCards();
			CurrentTurnClient = current.ClientID;

			//Debug.Log("Next Turn Client ID: " + CurrentTurnClient + "\nTurnNumber: " + CurrentTurn);
			if (CardPlayer.LocalPlayer != null)
			{
				CardPlayer.LocalPlayer.EnableInteractions = CardPlayer.LocalPlayer.ClientID == CurrentTurnClient;
				if (CardPlayer.LocalPlayer.ClientID == CurrentTurnClient)
				{
					OnStartTurn?.Invoke();
				}
				else
				{
					OnEndTurn?.Invoke();
				}
			}
			MapTransformInfo.Instance.SocketManager.SetTurn(CurrentTurnClient);

			current.DrawCard(1); //Next player is drawing one card
			current.PlayerStatistics.SetValue(CardPlayerStatType.Solar, MaxSolar);
			MapTransformInfo.Instance.SocketManager.TriggerEffect(EffectTrigger.OnRoundStart, current);


			RpcBroadcastEndTurn();
			//CurrentTurn++;
			//if (CurrentTurn >= CardPlayer.ServerPlayers.Count)
			//{
			//	CurrentTurn = 0;
			//}

			//CardPlayer current = CardPlayer.ServerPlayers[CurrentTurn];
			//CurrentTurnClient = current.ClientID;

			////Debug.Log("Next Turn Client ID: " + CurrentTurnClient + "\nTurnNumber: " + CurrentTurn);
			//if (CardPlayer.LocalPlayer != null)
			//	CardPlayer.LocalPlayer.EnableInteractions = CardPlayer.LocalPlayer.ClientID == CurrentTurnClient;
			//MapTransformInfo.Instance.SocketManager.SetTurn(CurrentTurnClient);
			//RpcBroadcastEndTurn(CurrentTurnClient);

			//current.DrawCard(1); //Next player is drawing one card



		}

		#endregion




	}
}
