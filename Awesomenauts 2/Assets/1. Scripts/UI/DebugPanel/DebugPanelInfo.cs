using AwsomenautsCardGame.Audio;
using AwsomenautsCardGame.Enums.Audio;
using AwsomenautsCardGame.Enums.Cards;
using AwsomenautsCardGame.Gameplay.Cards;
using AwsomenautsCardGame.Networking;
using VDFramework.VDUnityFramework.Singleton;
using UnityEngine;
using UnityEngine.UI;

namespace AwsomenautsCardGame.UI.DebugPanel
{
	public class DebugPanelInfo : Singleton<DebugPanelInfo>
	{

		public Camera CardPreviewCamera;
		public Image CardPreviewCameraImage;

		public Button EndTurnButton;
		public SolarDisplay SolarDisp;
		private bool init;
		public Text VersionText;
		

		private void Update()
		{
			if (!init && CardPlayer.LocalPlayer != null && CardPlayer.LocalPlayer.PlayerStatistics != null)
			{
				CardPlayer.LocalPlayer.PlayerStatistics.Register(CardPlayerStatType.Solar, OnUpdateSolar, true);
				init = true;
			}
			//UpdateVersionText(Application.version);
		}

		protected override void OnDestroy()
		{
			CardPlayer.LocalPlayer?.Awsomenaut?.Statistics.UnregisterAll();
			base.OnDestroy();
		}

		public void UpdateVersionText(string serverVersion)
		{
			if (serverVersion != Application.version)
			{
				CardNetworkManager.Exit = new CardNetworkManager.ExitFlag() { message = $"Server Version Mismatch.\nExpected {Application.version} got {serverVersion}." };
				CardNetworkManager.Instance.Stop();
			}
			VersionText.text = $"Client Version: {Application.version}\nServer Version: {serverVersion}";
		}

		private void OnUpdateSolar(object obj)
		{
			SolarDisp.SetSolar((int)obj);
		}

		//Connected to the UI in the Scene. Used to Disconnect from an active session.
		public void DisconnectNetwork()
		{
			AudioParameterManager.SetGlobalParameter("LowHealth", 0);
			AudioParameterManager.SetGlobalParameter("NexusDeath", 0);
			AudioParameterManager.SetGlobalParameter("IsInMenu", 1);
			AudioPlayer.StopEmitter(EmitterType.Ambient);
			
			CardNetworkManager.Instance?.Stop();
		}

		//Connected to the UI in the Scene. Used to Request to end the current turn.
		public void ClientRequestEndTurn()
		{
			CardPlayer.LocalPlayer?.EndTurn();
		}
	}
}
