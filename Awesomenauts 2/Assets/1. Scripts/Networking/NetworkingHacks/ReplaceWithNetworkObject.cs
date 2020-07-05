using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Byt3.Collections.Interfaces;
using Maps;
using Mirror;
using Networking;
using UnityEngine;


public class ReplaceWithNetworkObject : MonoBehaviour
{
	public SocketType SocketType;
	public SocketSide SocketSide;
	public GameObject Prefab;

	private CardSocket instance;

	public static Action ApplyConnections;

	public List<ReplaceWithNetworkObject> Sockets;

	void Awake()
	{

	}

	// Start is called before the first frame update
	void Start()
	{
		if (CardNetworkManager.Instance.IsServer)
		{
			GetSocket();
		}
		else
		{
			Destroy(gameObject);
		}
	}

	public CardSocket GetSocket()
	{
		if (instance != null) return instance;
		GameObject inst= Instantiate(Prefab, transform.position, transform.rotation);
		instance = inst.GetComponent<CardSocket>();
		instance.SocketSide = SocketSide;
		instance.SocketType = SocketType;

		NetworkServer.Spawn(inst);
		ApplyConnections += ApplyCons;
		return instance;
	}

	private void ApplyCons()
	{
		List<CardSocket> sockets = new List<CardSocket>();
		foreach (ReplaceWithNetworkObject replaceWithNetworkObject in Sockets)
		{
			sockets.Add(replaceWithNetworkObject.GetSocket());
		}
		instance.RpcSetNeighbours(sockets.Select(x => x.GetComponent<NetworkIdentity>()).ToArray());
		Destroy(gameObject);
	}

	// Update is called once per frame
	void Update()
	{

	}
}
