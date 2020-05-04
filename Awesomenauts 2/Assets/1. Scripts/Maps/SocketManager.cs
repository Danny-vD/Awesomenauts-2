using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Mirror;
using Networking;
using Player;
using UnityEngine;

namespace Maps {
	public class SocketManager : MonoBehaviour
	{
		/// <summary>
		/// Contains a List of Card Sockets and a Team Name
		/// </summary>
		[Serializable]
		public class CardTeamSocketData
		{
			public string name;
			public List<CardSocket> CardSockets;
		}

		/// <summary>
		/// Contains the Information on who has authority over with sockets.
		/// </summary>
		public List<CardTeamSocketData> CardSockets;
		/// <summary>
		/// Internal Representation of the CardSockets List
		/// Key = ClientID
		/// Value = Socket Data for the Player/Team
		/// </summary>
		private Dictionary<int, CardTeamSocketData> SocketData = null;

		// Start is called before the first frame update
		void Start()
		{
			if(!CardNetworkManager.Instance.IsServer)return;



			//foreach (CardTeamSocketData cardTeamSocketData in CardSockets)
			//{
			//	for (int i = 0; i < cardTeamSocketData.CardSockets.Count; i++)
			//	{
			//		CardSocket cardSocket = cardTeamSocketData.CardSockets[i];
			//		CardSocket cs = Instantiate(cardSocket);
			//		Destroy(cardSocket);
			//		cardTeamSocketData.CardSockets[i] = cs;
			//		NetworkServer.Spawn(cs.gameObject);
			//	}
			//}
		}

		public void RegisterSocket(int id, CardSocket socket)
		{
			if(!socketMap.ContainsKey(id))socketMap.Add(id, new List<CardSocket>{socket});
			else socketMap[id].Add(socket);
		}

		private Dictionary<int, List<CardSocket>> socketMap=new Dictionary<int, List<CardSocket>>();

		/// <summary>
		/// Maps the ClientIDS to the Corresponding Team IDS
		/// </summary>
		/// <param name="clientIDs"></param>
		/// <param name="teamIDs"></param>
		public void AddPlayers(int[] clientIDs, int[] teamIDs)
		{
			if(SocketData==null)
				SocketData = new Dictionary<int, CardTeamSocketData>();

			

			for (int i = 0; i < clientIDs.Length; i++)
			{
				AddPlayer(clientIDs[i], teamIDs[i]);
			}
			foreach (KeyValuePair<int, List<CardSocket>> keyValuePair in socketMap)
			{
				SocketData[keyValuePair.Key].CardSockets=keyValuePair.Value;
			}

			for (int i = 0; i < clientIDs.Length; i++)
			{
				foreach (CardSocket cardSocket in CardSockets[teamIDs[i]].CardSockets)
				{
					cardSocket.SetClientID(clientIDs[i]);
				}
			}
			socketMap.Clear();

		}

		/// <summary>
		/// Maps a PlayerID to a TeamID
		/// </summary>
		/// <param name="clientID"></param>
		/// <param name="teamID"></param>
		private void AddPlayer(int clientID, int teamID)
		{
			if (!SocketData.ContainsKey(clientID))
			{
				SocketData.Add(clientID, CardSockets[teamID]);
				//foreach (CardSocket cardSocket in CardSockets[teamID].CardSockets)
				//{
				//	cardSocket.SetClientID(clientID);
				//}
			}
		}

		/// <summary>
		/// Sets the turn of the current Client ID.
		/// Animates the Sockets of the current turn client and stops animations of all other clients
		/// </summary>
		/// <param name="clientID"></param>
		public void SetTurn(int clientID)
		{
			foreach (KeyValuePair<int, CardTeamSocketData> cardTeamSocketData in SocketData)
			{
				SetActive(cardTeamSocketData.Value.CardSockets, cardTeamSocketData.Key == clientID);
			}
		}

		/// <summary>
		/// Can be used to Check if a CardSocket belongs to the Client
		/// </summary>
		/// <param name="clientID"></param>
		/// <param name="objectToCheck"></param>
		/// <returns></returns>
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
}
