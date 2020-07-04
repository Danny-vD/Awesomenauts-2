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
		public bool CanStartGame => !GameStarted && !gameStarting;
		public bool GameStarted;
		private bool gameStarting;

		private int MaxSolar = 1;
		private int CurrentTurn = -1;
		public int CurrentTurnClient { get; private set; }

		private float TimeStamp;
		public static BoardLogic Logic;
		public Action OnEndTurn;
		public Action OnStartTurn;

		private bool[] ReadyList;

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


			if (gameStarting && ReadyList.All(x => x))
			{
				gameStarting = false;
				GameStarted = true;
				FinalizeGameStart();
			}

		}

		private void FinalizeGameStart()
		{
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

				CardPlayer.ServerPlayers[i].RpcSetAwsomenaut(awsomenaut.netIdentity);
				CardPlayer.ServerPlayers[i].SetAwsomenaut(awsomenaut.netIdentity);


				//Set Turrets/Awsomenauts for all clients
			}

			for (int i = 0; i < CardPlayer.ServerPlayers.Count; i++)
			{
				if (i != 0)
				{
					CardPlayer.ServerPlayers[i].DrawCard(CardNetworkManager.Instance.StartingCards);
					CardPlayer.ServerPlayers[i].DrawCard(CardNetworkManager.Instance.CompensationCard);
				}
				else
				{
					CardPlayer.ServerPlayers[i].DrawCard(CardNetworkManager.Instance.StartingCards - 1);
				}
			}


			ServerEndTurn();
		}

		#region Client Remote Procedure Calls


		//Gets Called on all Clients to end the current turn
		//next is the next players ID that has the turn.
		[ClientRpc]
		public void RpcBroadcastEndTurn()
		{
			if (!isServer)
				EndTurn();
		}

		private void EndTurn()
		{

			if (CurrentTurn != -1)
			{
				CardPlayer.ServerPlayers.ForEach(x => MapTransformInfo.Instance.SocketManager.TriggerEffect(EffectTrigger.AfterRoundEnd,
					x));
			}
			CurrentTurn++;
			if (CurrentTurn >= CardPlayer.ServerPlayers.Count)
			{
				CurrentTurn = 0;
				MaxSolar++;
			}
			if (MaxSolar >= 10) MaxSolar = 10;

			CardPlayer current = CardPlayer.ServerPlayers[CurrentTurn];
			current.ClearUsedCards();
			CurrentTurnClient = current.ClientID;

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

			current.PlayerStatistics.SetValue(CardPlayerStatType.Solar, MaxSolar);

			CardPlayer.ServerPlayers.ForEach(x => MapTransformInfo.Instance.SocketManager.TriggerEffect(EffectTrigger.OnRoundStart, x));

		}

		#endregion

		#region Server Only Calls

		[Server]
		public void SetClientReady(int clientID)
		{
			ReadyList[clientID] = true;
		}

		//Starts the Game
		//Sets up the client ids and the teamids
		[Server]
		public void StartGame()
		{
			Debug.Log("Starting Game.");
			gameStarting = true;
			if (GameInitializer.Data.DebugInfo.StartSolar != -1)
			{
				MaxSolar = GameInitializer.Data.DebugInfo.StartSolar;
			}
			RpcSetServerVersion(Application.version);
			int[] clientIDs = new int[CardPlayer.ServerPlayers.Count];
			ReadyList = new bool[CardPlayer.ServerPlayers.Count];
			Debug.Log("Sending ClientIDs:");
			for (int i = 0; i < CardPlayer.ServerPlayers.Count; i++)
			{
				Debug.Log(CardPlayer.ServerPlayers[i].ClientID + ":" + i);
				clientIDs[i] = i;
			}

			MapTransformInfo.Instance.SocketManager.AddPlayers(clientIDs);

			for (int i = 0; i < CardPlayer.ServerPlayers.Count; i++)
			{
				CardPlayer.ServerPlayers[i].RpcSetClientPlayerID(clientIDs[i], clientIDs);
			}


			//Debug.Log("Player Count: "+ CardPlayer.ServerPlayers.Count);

		}
		[ClientRpc]
		private void RpcSetServerVersion(string version)
		{
			Debug.Log($"Server Version: {version}");

			DebugPanelInfo.Instance.UpdateVersionText(version);
		}

		//Ends the turn
		[Server]
		public void ServerEndTurn()
		{
			if (!GameStarted) return;


			EndTurn();

			RpcBroadcastEndTurn();



		}

		#endregion




	}
}
