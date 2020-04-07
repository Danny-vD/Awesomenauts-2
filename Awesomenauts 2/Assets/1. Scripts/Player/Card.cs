using System;
using Mirror;
using Networking;
using UnityEngine;

public class Card : NetworkBehaviour
{
	public MeshRenderer CoverUpRenderer;

	public EntityStatistics Statistics;
	// Start is called before the first frame update
	void Start()
	{
		SetCoverState(isClient && !hasAuthority);
	}

	[TargetRpc]
	public void TargetSendStats(NetworkConnection identity, int[] statType, int[] dataType, float[] data)
	{
		Debug.Log("Received Stats");
		Statistics = CardEntry.FromNetwork(new Tuple<int[], int[], float[]>(statType, dataType, data));
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
}
