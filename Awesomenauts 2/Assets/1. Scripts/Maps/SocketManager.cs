using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class SocketManager : MonoBehaviour
{
    [Serializable]
    public struct CardTeamSocketData
    {
        public string name;
        public List<CardSocket> CardSockets;
    }

    public List<CardTeamSocketData> CardSockets;
    private Dictionary<int, CardTeamSocketData> SocketData = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void AddPlayers(int[] clientIDs, int[] teamIDs)
    {
        if(SocketData==null)
            SocketData = new Dictionary<int, CardTeamSocketData>();
        for (int i = 0; i < clientIDs.Length; i++)
        {
            Debug.Log("Mapping Client ID:" + clientIDs[i] + " to TeamID: " + teamIDs[i]);
            AddPlayer(clientIDs[i], teamIDs[i]);
        }
    }

    private void AddPlayer(int clientID, int teamID)
    {
        if (!SocketData.ContainsKey(clientID))
        {
            SocketData.Add(clientID, CardSockets[teamID]);
        }
    }

    public void SetTurn(int clientID)
    {
        foreach (KeyValuePair<int, CardTeamSocketData> cardTeamSocketData in SocketData)
        {
            SetActive(cardTeamSocketData.Value.CardSockets, cardTeamSocketData.Key == clientID);
        }
    }

    public bool IsFromTeam(int clientID, Transform objectToCheck)
    {
        return SocketData.ContainsKey(clientID) &&
               SocketData[clientID].CardSockets.Count(x => x.transform == objectToCheck) != 0;
    }

    private void SetActive(List<CardSocket> sockets, bool active)
    {
        for (int i = 0; i < sockets.Count; i++)
        {
            sockets[i].SetActive(active);
        }
    }
}
