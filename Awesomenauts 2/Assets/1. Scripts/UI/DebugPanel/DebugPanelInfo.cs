using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DebugPanelInfo : MonoBehaviour
{

    public Button EndTurnButton;

    public void RegisterEndTurn(UnityAction action)
    {
        EndTurnButton.onClick.AddListener(action);
    }
}
