using System;
using System.Threading;
using System.Threading.Tasks;
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
		private float TimeStamp;
		private int Tries;
		private bool connectSuccess;
		private bool connecting;
		private bool instanceConnecting;
		private MasterServerAPI.ConnectionEvents connectionEvents;
		public MasterServerInfo Info;

		private void Start()
		{

			PacketSerializer.Serializer.AddSerializer(new ClientHeartBeatSerializer(), typeof(ClientHeartBeatPacket));
			PacketSerializer.Serializer.AddSerializer(new ClientHandshakeSerializer(), typeof(ClientHandshakePacket));
			PacketSerializer.Serializer.AddSerializer(new ClientInstanceReadySerializer(), typeof(ClientInstanceReadyPacket));


			if (Info == null) Info = new MasterServerInfo();
		}

		public void ConnectToServer(MasterServerAPI.ConnectionEvents events)
		{
			if (connecting) return;
			connectionEvents = events;
			connecting = true;
			connectionEvents.OnError += (MatchMakingErrorCode e, Exception ex) =>
			{
				connecting = false;
				Debug.Log("Error Code: " + e);
				Debug.LogException(ex);
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
            Debug.Log("AAAAAAAAA");
			instanceConnecting = true;
			connectSuccess = false;


			TimeStamp = Time.realtimeSinceStartup + Info.ConnectInstanceTimeout / 1000f;
			Tries++;
		}

		public void SetConnectionSuccess()
		{
			instanceConnecting = false;
			connectSuccess = true;
			tokenSource.Cancel();
			tokenSource = new CancellationTokenSource();
			connecting = false;
			TimeStamp = 0;
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