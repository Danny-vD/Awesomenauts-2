using System;
using System.Threading;
using System.Threading.Tasks;
using Byt3.Serialization;
using MasterServer.Client;
using MasterServer.Common.Networking;
using MasterServer.Common.Networking.Packets;
using MasterServer.Common.Networking.Packets.Serializers;
using Mirror;
using UnityEngine;

namespace Networking
{
	public class MasterServerComponent : MonoBehaviour
	{
		private CancellationTokenSource tokenSource = new CancellationTokenSource();
		private int Tries;
		private bool running;
		private bool connectSuccess;
		private bool connecting;
		private bool instanceConnecting;
		private MasterServerAPI.ConnectionEvents connectionEvents;
		public MasterServerInfo Info;

		private void Start()
		{

			Byt3Serializer.AddSerializer<ClientHeartBeatPacket>(new ClientHeartBeatSerializer());
			Byt3Serializer.AddSerializer<ClientHandshakePacket>(new ClientHandshakeSerializer());
			Byt3Serializer.AddSerializer<ClientInstanceReadyPacket>(new ClientInstanceReadySerializer());


			if (Info == null) Info = new MasterServerInfo();
		}

		public void AbortQueue()
		{
            if(running)
            {
	            tokenSource.Cancel();
                tokenSource = new CancellationTokenSource();
            }

		}

		public void ConnectToServer(MasterServerAPI.ConnectionEvents events)
		{
			if (connecting || running) return;
			running = true;
			connectionEvents = events;
			connecting = true;
			connectionEvents.OnError += (MatchMakingErrorCode e, Exception ex) =>
			{
				connecting = false;

				running = false;
				Debug.Log("Error Code: " + e);
				if (ex != null)
					Debug.LogWarning("Received Exception: " + ex);
			};
			connectionEvents.OnStatusUpdate += Debug.Log;
			connectionEvents.OnSuccess += ConnectToGameInstance;
			MasterServerAPI.QueueAsync(connectionEvents, Info.Address.IP, Info.Address.Port, tokenSource.Token).Start();
		}


		private void ConnectToGameInstance(MasterServerAPI.ServerInstanceResultPacket result)
		{
			EndPointInfo ep =
				new EndPointInfo(Info.Address.IP, result.Port);
			CardNetworkManager.Instance.CurrentEndPoint = ep;
			instanceConnecting = true;
			connectSuccess = false;
			Tries++;
		}

		public void SetConnectionSuccess()
		{
			Tries = 0;
			instanceConnecting = false;
			connectSuccess = true;
			tokenSource.Cancel();
			tokenSource = new CancellationTokenSource();
			connecting = false;
			running = false;
		}

		private void Update()
		{
			if (instanceConnecting && !connectSuccess)
			{
				if (!NetworkClient.active)
				{
					CardNetworkManager.Instance.StartClient();
					connectionEvents.OnStatusUpdate($"Connecting To Server try {Tries}...");
					Tries++;
				}
				else if (NetworkClient.isConnected)
				{
					SetConnectionSuccess();
				}
			}
		}
	}
}