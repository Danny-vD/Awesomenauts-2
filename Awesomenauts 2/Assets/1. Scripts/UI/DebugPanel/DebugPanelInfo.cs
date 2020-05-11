using Networking;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI.DebugPanel
{
	public class DebugPanelInfo : MonoBehaviour
	{

		public Button EndTurnButton;
		public Text SolarText;
		private bool init;
		public Text VersionText;
		public static DebugPanelInfo instance;

		void Awake()
		{

			instance = this;



		}

		private void Update()
		{
			if (!init && CardPlayer.LocalPlayer != null)
			{
				CardPlayer.LocalPlayer.PlayerStatistics.Register(CardPlayerStatType.Solar, OnUpdateSolar);
				OnUpdateSolar(CardPlayer.LocalPlayer.PlayerStatistics.GetValue<int>(CardPlayerStatType.Solar));
				init = true;
			}
			//UpdateVersionText(Application.version);
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
			SolarText.text = $"Solar: {(int)obj}/10";
		}

		//Connected to the UI in the Scene. Used to Disconnect from an active session.
		public void DisconnectNetwork()
		{
			CardNetworkManager.Instance.Stop();
		}

		//Connected to the UI in the Scene. Used to Request to end the current turn.
		public void ClientRequestEndTurn()
		{
			CardPlayer.LocalPlayer.EndTurn();
		}
	}
}
