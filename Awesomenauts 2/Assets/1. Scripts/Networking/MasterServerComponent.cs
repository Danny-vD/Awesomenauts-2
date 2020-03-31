using System.Threading;
using System.Threading.Tasks;
using MasterServer.Client;
using MasterServer.Common;
using MasterServer.Common.Packets;
using MasterServer.Common.Packets.Serializers;
using Mirror;
using UnityEngine;

namespace Networking
{
	public class MasterServerComponent : MonoBehaviour
	{

		private Task<MasterServerAPI.ServerHandshakePacket> handShakeTask = null;
		private Task<MasterServerAPI.ServerInstanceResultPacket> resultTask = null;
		private CancellationTokenSource tokenSource = new CancellationTokenSource();
		private float TimeStamp;
		private int Tries;
		private ConnectionState cs = ConnectionState.Idle;

		private ConnectionState connectionState
		{
			get => cs;
			set
			{
				cs = value;
				if (cs == ConnectionState.Idle)
					Debug.Log("New Value: " + cs);
			}
		}

		public MasterServerInfo Info;

		private void Start()
		{

			PacketSerializer.Serializer.AddSerializer(new ClientHeartBeatSerializer(), typeof(ClientHeartBeatPacket));
			PacketSerializer.Serializer.AddSerializer(new ClientHandshakeSerializer(), typeof(ClientHandshakePacket));
			PacketSerializer.Serializer.AddSerializer(new ClientInstanceReadySerializer(), typeof(ClientInstanceReadyPacket));


			if (Info == null) Info = new MasterServerInfo();
		}

		public void ConnectToServer()
		{
			handShakeTask = MasterServerAPI.BeginConnectionAsync(Info.Address.IP, Info.Address.Port);
			handShakeTask.Start();
			connectionState = ConnectionState.Connecting;
			onMasterServerStartConnecting?.Invoke();
		}

		private bool TaskFinishedSuccessful(Task t)
		{
			return t != null && t.IsCompleted && !t.IsFaulted;
		}

		private bool TaskUnfinished(Task t)
		{
			return t != null && !t.IsCompleted;
		}

		private bool TaskFailed(Task t)
		{
			return t != null && t.IsFaulted;
		}

		private void ResetTasks()
		{
			tokenSource.Cancel();
			tokenSource = new CancellationTokenSource();
			handShakeTask?.Dispose();
			resultTask?.Dispose();
			resultTask = null;
			handShakeTask = null;
			connectionState = ConnectionState.Idle;
			TimeStamp = 0;
			onMasterServerReset?.Invoke();
		}

		private void ProcessTaskEnd(Task t, ConnectionState nextState)
		{

			if (TaskUnfinished(t)) return;
			if (TaskFinishedSuccessful(t))
			{
				Debug.Log(t.Status);
				connectionState = nextState;
				return;
			}
			Debug.LogError(t.Exception.InnerExceptions[0]);
			ResetTasks(); //Reset

		}

		public delegate void OnMasterServerReset();

		public delegate void OnMasterServerStartConnecting();

		public delegate void OnMasterServerStartQueue();

		public delegate void OnMasterServerFoundMatch(EndPointInfo serverInstance);

		public delegate void OnMasterServerConnectToInstance(int tries);

		public OnMasterServerReset onMasterServerReset;
		public OnMasterServerStartConnecting onMasterServerStartConnecting;
		public OnMasterServerStartQueue onMasterServerStartQueue;
		public OnMasterServerFoundMatch onMasterServerFoundMatch;
		public OnMasterServerConnectToInstance onMasterServerConnectToInstance;

		private void ConnectToGameInstance()
		{
			CardNetworkManager.Instance.StartClient();
			TimeStamp = Time.realtimeSinceStartup + Info.ConnectInstanceTimeout / 1000f;
			Tries++;
			onMasterServerConnectToInstance?.Invoke(Tries);
		}

		//public void SetConnectionSuccess()
		//{
		//	tokenSource.Cancel();
		//	tokenSource = new CancellationTokenSource();
		//	handShakeTask?.Dispose();
		//	resultTask?.Dispose();
		//	resultTask = null;
		//	handShakeTask = null;
		//	connectionState = ConnectionState.Idle;
		//	TimeStamp = 0;
		//}

		private void Update()
		{
			if (connectionState == ConnectionState.Idle) return;
			if (connectionState == ConnectionState.Connecting)
			{
				ProcessTaskEnd(handShakeTask, ConnectionState.Connected);
			}

			if (connectionState == ConnectionState.Connected)
			{
				resultTask = MasterServerAPI.FindMatchAsync(handShakeTask.Result, tokenSource.Token);
				resultTask.Start();
				connectionState = ConnectionState.Queued;
				onMasterServerStartQueue?.Invoke();
			}

			if (connectionState == ConnectionState.Queued)
			{
				ProcessTaskEnd(resultTask, ConnectionState.FoundMatch);
			}

			if (connectionState == ConnectionState.FoundMatch)
			{
				EndPointInfo ep =
					new EndPointInfo(Info.Address.IP, resultTask.Result.Port);
				CardNetworkManager.Instance.CurrentEndPoint = ep;
				//CardNetworkManager.Instance.RefreshEndPoint();
				connectionState = ConnectionState.ReconnectLoop;
				onMasterServerFoundMatch?.Invoke(ep);
				handShakeTask.Dispose();
				resultTask.Dispose();
				resultTask = null;
				handShakeTask = null;
				ConnectToGameInstance();
			}

			if (connectionState == ConnectionState.ReconnectLoop && !NetworkClient.active)
			{
				if (Tries >= Info.ConnectInstanceTries)
				{
					ResetTasks();
					return;
				}
                
				ConnectToGameInstance();
			}
		}
	}
}