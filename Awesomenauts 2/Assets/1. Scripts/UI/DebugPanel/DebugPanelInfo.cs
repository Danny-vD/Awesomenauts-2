using Networking;
using UnityEngine;
using UnityEngine.UI;

public class DebugPanelInfo : MonoBehaviour
{

    public Button EndTurnButton;
    public Text SolarText;
    private bool init;


    private void Update()
    {
	    if (!init && CardPlayer.LocalPlayer != null)
	    {
			CardPlayer.LocalPlayer.PlayerStatistics.Register(CardPlayerStatType.Solar, OnUpdateSolar);
			OnUpdateSolar(CardPlayer.LocalPlayer.PlayerStatistics.GetValue<int>(CardPlayerStatType.Solar));
			init = true;
	    }
    }

    private void OnUpdateSolar(object obj)
    {
	    SolarText.text = $"Solar: {(int) obj}/10";
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
