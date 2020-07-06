using System;
using System.Collections.Generic;
using System.Linq;
using Assets._1._Scripts.ScriptableObjects.Effects;
using Player;
using UnityEngine;

namespace Maps
{
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


		public void RegisterSocket(int id, CardSocket socket)
		{
			if (!socketMap.ContainsKey(id)) socketMap.Add(id, new List<CardSocket> { socket });
			else socketMap[id].Add(socket);
		}

		private Dictionary<int, List<CardSocket>> socketMap = new Dictionary<int, List<CardSocket>>();

		/// <summary>
		/// Maps the ClientIDS to the Corresponding Team IDS
		/// </summary>
		/// <param name="clientIDs"></param>
		/// <param name="teamIDs"></param>
		public void AddPlayers(int[] clientIDs)
		{
			if (SocketData == null)
				SocketData = new Dictionary<int, CardTeamSocketData>();



			for (int i = 0; i < clientIDs.Length; i++)
			{
				AddPlayer(clientIDs[i]);
			}


			foreach (KeyValuePair<int, List<CardSocket>> keyValuePair in socketMap)
			{
				SocketData[SocketData.ElementAt(keyValuePair.Key).Key].CardSockets = keyValuePair.Value;
			}

			for (int i = 0; i < clientIDs.Length; i++)
			{
				foreach (CardSocket cardSocket in CardSockets[clientIDs[i]].CardSockets)
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
		private void AddPlayer(int clientID)
		{
			if (!SocketData.ContainsKey(clientID))
			{
				SocketData.Add(clientID, CardSockets[clientID]);
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

		public void TriggerEffect(EffectTrigger trigger, CardPlayer player)
		{
			foreach (KeyValuePair<int, CardTeamSocketData> cardTeamSocketData in SocketData)
			{
				foreach (CardSocket valueCardSocket in cardTeamSocketData.Value.CardSockets)
				{
					if (valueCardSocket.HasCard && valueCardSocket.DockedCard.Statistics.GetValue<int>(CardPlayerStatType.TeamID) == player.ClientID)
					{
						valueCardSocket.DockedCard.EffectManager.InvokeEffects(trigger, valueCardSocket, valueCardSocket, valueCardSocket.DockedCard);
					}
				}
			}
		}

		public CardSocket[] GetCardSockets()
		{
			List<CardSocket> ret = new List<CardSocket>();
			foreach (KeyValuePair<int, CardTeamSocketData> cardTeamSocketData in SocketData)
			{
				foreach (CardSocket valueCardSocket in cardTeamSocketData.Value.CardSockets)
				{
					ret.Add(valueCardSocket);
				}
			}

			return ret.ToArray();
		}

		public CardSocket[] GetSocketsOnSide(SocketSide side)
		{
			List<CardSocket> ret = new List<CardSocket>();
			foreach (KeyValuePair<int, CardTeamSocketData> cardTeamSocketData in SocketData)
			{
				foreach (CardSocket valueCardSocket in cardTeamSocketData.Value.CardSockets)
				{
					if ((side & valueCardSocket.SocketSide) == side && ret.All(x => valueCardSocket.netIdentity != x.netIdentity)) ret.Add(valueCardSocket);
				}
			}

			return ret.ToArray();
		}
	}
}
