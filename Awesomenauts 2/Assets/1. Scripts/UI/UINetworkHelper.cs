using System;
using System.Reflection;
using System.Threading;
using DataObjects;
using Networking;
using Utility;
using MasterServer.Client;
using MasterServer.Common.Networking.Packets;
using UnityEngine;
using UnityEngine.UI;
using VDFramework.Singleton;

namespace UI
{
	public class UINetworkHelper : Singleton<UINetworkHelper>
	{
		public struct FieldInformation
		{
			public string path;
			public FieldInfo info;
			public object reference;
		}

		public InputField[] fields;

		CancellationTokenSource ts = new CancellationTokenSource();



		public GameObject MainMenu;
		public GameObject LoadingScreen;
		private MatchMakingErrorCode ErrorCode = MatchMakingErrorCode.None;
		private Exception Exception;
		public Text QueueStatus;
		private string StatusText;
		private float TimeStamp;
		private bool DisplayTimestamp;
		private bool FindMatchOnlyDelayFlag;

		private EndPointInfo ServerInstance;
		private float StartTimestamp;
		private float LastUpdateTimestamp;
		private string TimeStampUI =>
			DisplayTimestamp ? $"({Mathf.RoundToInt(Time.realtimeSinceStartup - TimeStamp).ToString()})" : "";

		public Button buttonFindMatch;
		private bool UpdateQueueStatus;
		private bool UpdateErrorStatus;

		void Start()
		{
			if (CardNetworkManager.Exit != null)
			{
				//MainMenu.SetActive(false);
				CardNetworkManager.Instance.ExUI.SetException(new Exception("Game Error: " + CardNetworkManager.Exit.message));
				//UpdateErrText("Game Error", CardNetworkManager.Exit.message);
				CardNetworkManager.Exit = null;
			}


			foreach (InputField inputField in fields)
			{
				inputField.SetTextWithoutNotify(GameInitializer.Data.Network.DefaultAddress.IP);
			}
			if (GameInitializer.Data.Network.FindMatchOnly && GameInitializer.Data.Network.FindMatchOnlyDelay == 0) buttonFindMatch.onClick.Invoke();
			StartTimestamp = Time.realtimeSinceStartup;
		}

		private void OnDestroy()
		{
			ts.Cancel();

		}

		public void AbortQueue()
		{
			GameInitializer.Master.AbortQueue();
		}

		private void OnMasterServerFoundMatch(MasterServerAPI.ServerInstanceResultPacket serverinstance)
		{
			EndPointInfo ep = new EndPointInfo(GameInitializer.Master.Info.Address.IP, serverinstance.Port);
			ServerInstance = ep;
			TimeStamp = LastUpdateTimestamp;
			DisplayTimestamp = false;
			UpdateQueueStatus = true;
			SetText("Found Match on Instance: " + serverinstance);
		}



		//Passthrough for the UI. Since the Network Manager is not loaded in the same scene we need to pass it through something static for the UI to work.

		private void Update()
		{
			LastUpdateTimestamp = Time.realtimeSinceStartup;
			if (!FindMatchOnlyDelayFlag && GameInitializer.Data.Network.FindMatchOnly && GameInitializer.Data.Network.FindMatchOnlyDelay != 0)
			{
				float add = GameInitializer.Data.Network.FindMatchOnlyDelay / 1000f;
				if (StartTimestamp + add < Time.realtimeSinceStartup)
				{
					FindMatchOnlyDelayFlag = true;
					buttonFindMatch.onClick.Invoke();
				}
			}

			if (UpdateQueueStatus)
			{
				UpdateQueueText(StatusText);
			}

			if (UpdateErrorStatus)
			{
				UpdateErrText(Exception, ErrorCode);
			}
		}

		private void SetText(string text)
		{
			StatusText = text;
			TimeStamp = LastUpdateTimestamp;
		}

		private void UpdateQueueText(string text)
		{
			QueueStatus.text =
				$"{text}{TimeStampUI}";
		}
		private void UpdateErrText(Exception ex, MatchMakingErrorCode e)
		{
			UpdateErrorStatus = false;
			if (ex != null)
			{
				CardNetworkManager.Instance.ExUI.SetException(ex, Enum.GetName(e.GetType(), e));
			}
			else
			{
				Exception exx = new Exception(Enum.GetName(e.GetType(), e));
				CardNetworkManager.Instance.ExUI.SetException(exx, Enum.GetName(e.GetType(), e));
			}

			LoadingScreen?.SetActive(false);
		}

		#region Button Functions

		public void StartClient()
		{
			CardNetworkManager.Instance.ApplyEndPoint();
			CardNetworkManager.Instance.StartClient();
		}

		public void StartHost()
		{
			CardNetworkManager.Instance.ApplyEndPoint();
			CardNetworkManager.Instance.StartHost();
		}


		public void StartServer()
		{
			CardNetworkManager.Instance.ApplyEndPoint();
			CardNetworkManager.Instance.StartServer();
		}


		public void IPChanged(string text)
		{
			if (!string.IsNullOrEmpty(text))
			{
				EndPointInfo ep = CardNetworkManager.Instance.CurrentEndPoint;
				ep.IP = text;
				CardNetworkManager.Instance.CurrentEndPoint = ep;
			}
		}

		public void FindGame()
		{
			UpdateQueueStatus = true;
			DisplayTimestamp = true;
			SetText("Connecting to Master Server..");

			MasterServerAPI.ConnectionEvents ev = new MasterServerAPI.ConnectionEvents();
			ev.OnError += (MatchMakingErrorCode e, Exception ex) =>
			{
				ErrorCode = e;
				Exception = ex;
				DisplayTimestamp = false;
				UpdateQueueStatus = true;
				UpdateErrorStatus = true;
			};
			ev.OnStatusUpdate += SetText;
			ev.OnSuccess += OnMasterServerFoundMatch;

			GameInitializer.Master.ConnectToServer(ev);
		}
		#endregion


	}
}
