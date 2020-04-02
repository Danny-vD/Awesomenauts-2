using Networking;
using UnityEngine;
using UnityEngine.UI;

public class DebugPanelInfo : MonoBehaviour
{

    public Button EndTurnButton;


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
