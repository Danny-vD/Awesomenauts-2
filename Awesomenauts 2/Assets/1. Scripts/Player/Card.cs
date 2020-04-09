using System;
using Networking;
using Mirror;
using UnityEngine;

namespace Player {
	public enum CardState
	{
		OnDeck,
		OnHand,
		OnBoard,
		OnGrave,
	}
	public class Card : NetworkBehaviour
	{
		public MeshRenderer CoverUpRenderer;

	

		public EntityStatistics Statistics;
		public bool StatisticsValid { get; private set; }
		public CardState CardState { get; private set; } = CardState.OnDeck;

		// Start is called before the first frame update
		void Start()
		{
			SetCoverState(isClient && !hasAuthority);
		}

		[TargetRpc]
		public void TargetSendStats(NetworkConnection identity, byte[] data)
		{
			Debug.Log("Received Stats");
			Statistics = CardEntry.FromNetwork(data);
			StatisticsValid = true;
		}

		/// <summary>
		/// Sets the Cover Up Renderer Active or Inactive.
		/// </summary>
		/// <param name="covered"></param>
		public void SetCoverState(bool covered)
		{
			//Debug.Log("Enable Cover Up Renderer: " + covered);
			CoverUpRenderer.enabled = covered;
		}

		public void SetState(CardState state)
		{
			CardState = state;

			if (state == CardState.OnBoard)
			{
				//Reverse the Turning over
				Quaternion turnOverRot = Quaternion.AngleAxis(-180, transform.up);
				transform.rotation *= turnOverRot;
			}
		}
	}
}