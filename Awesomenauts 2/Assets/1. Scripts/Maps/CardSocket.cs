using System;
using System.Collections.Generic;
using System.Linq;
using AStar;
using Mirror;
using Networking;
using Player;
using UnityEngine;

namespace Maps
{
	public enum SocketType { TurretLeft, TurretRight, Default, Awsomenaut }
	public class CardSocket : NetworkBehaviour, IComparable<CardSocket>, IEquatable<CardSocket>
	{


		public SocketType SocketType = SocketType.Default;
		// Properties

		/// <summary>
		/// Gets the Total cost of the Node.
		/// The Current Costs + the estimated costs.
		/// </summary>
		public double TotalCost => CurrentCost + EstimatedCost;

		/// <summary>
		/// Gets or sets the Distance between this node and the target node.
		/// </summary>
		public double EstimatedCost { get; set; }

		/// <summary>
		/// Gets a value indicating whether how costly it is to traverse over this node.
		/// </summary>
		public double TraversalCostMultiplier => 1;

		/// <summary>
		/// Gets or sets a value indicating whether to go from the start node to this node.
		/// </summary>
		public double CurrentCost { get; set; }

		/// <summary>
		/// Gets or sets the state of the Node
		/// Can be Unconsidered(Default), Open and Closed.
		/// </summary>
		public NodeState State { get; set; }

		/// <summary>
		/// Gets a value indicating whether the node is traversable.
		/// </summary>
		public bool Traversable { get; } = true;

		/// <summary>
		/// Gets or sets a list of all connected nodes.
		/// </summary>
		public CardSocket[] ConnectedNodes { get; set; }

		/// <summary>
		/// Gets or sets he "previous" node that was processed before this node.
		/// </summary>
		public CardSocket Parent { get; set; }



		/// <summary>
		/// Compares the Nodes based on their total costs.
		/// Total Costs: A* Pathfinding.
		/// Current: Djikstra Pathfinding.
		/// Estimated: Greedy Pathfinding.
		/// </summary>
		/// <param name="other">The other node.</param>
		/// <returns>A comparison between the costs.</returns>
		public int CompareTo(CardSocket other) => TotalCost.CompareTo(other.TotalCost);



		/// <summary>
		/// Override for IEquatable.
		/// </summary>
		/// <param name="other">The object to be checked against.</param>
		/// <returns>True if Equal, False if not Equal.</returns>
		public bool Equals(CardSocket other) => CompareTo(other) == 0;

		/// <summary>
		/// Returns the distance to the other node.
		/// </summary>
		/// <param name="other">The other node.</param>
		/// <returns>Distance between this and other.</returns>
		public double DistanceTo(CardSocket other)
		{
			// Since we are only using the distance in comparison with other distances, we can skip using Math.Sqrt
			return Vector3.Distance(other.transform.position, transform.position);
		}

		public SocketSide SocketSide;
		private float origY;


		public bool HasCard => DockedCard != null;


		public int TeamID;
		public float yScale;
		public float yOffset;
		public float yCardOffset;
		public float timeScale;
		public float timeOffset;

		private bool Active;
		private bool Initialized;

		public int ClientID { get; private set; }

		public Card DockedCard { get; private set; }

		public void SetClientID(int id)
		{
			ClientID = id;
		}

		[ClientRpc]
		public void RpcSetNeighbours(NetworkIdentity[] neighboard)
		{
			SetNeighbours(neighboard.Select(x => x.GetComponent<CardSocket>()).ToArray());
		}



		public void SetNeighbours(CardSocket[] neighboard)
		{
			ConnectedNodes = neighboard;
		}

		// Start is called before the first frame update
		void Start()
		{
			origY = transform.position.y;
			MapTransformInfo.Instance.SocketManager.RegisterSocket(TeamID, this);
		}

		/// <summary>
		/// Activates or deactivates the Socket Movement
		/// </summary>
		/// <param name="active"></param>
		public void SetActive(bool active)
		{
			Active = active;
			if (!Active) ResetPositions();
		}


		[ClientRpc]
		public void RpcSetSocketSide(SocketSide side)
		{
			Initialized = true;
			SetSocketSide(side);
		}
		public void SetSocketSide(SocketSide side)
		{
			SocketSide = side;
			Lane.AddSocket(this);
		}

		/// <summary>
		/// Docks a transform to the Socket
		/// This way the transform will be moving with the Socket.
		/// </summary>
		/// <param name="dockedTransform"></param>
		public void DockCard(Card dockedTransform)
		{
			DockedCard?.SetSocket(null);
			DockedCard = dockedTransform;
			DockedCard.transform.rotation = transform.rotation;
			DockedCard.transform.Rotate(Vector3.right * -90, Space.Self);
			DockedCard.transform.Rotate(Vector3.forward * 90, Space.Self);
			DockedCard?.SetSocket(this);

			ResetPositions();
		}

		[Command]
		public void CmdUnDockCard()
		{
			RpcUnDockCard();
		}

		[Command]
		public void CmdDockCard(NetworkIdentity dockedTransform)
		{
			RpcDockCard(dockedTransform);
		}

		[ClientRpc]
		public void RpcUnDockCard()
		{
			DockCard(null);
		}

		[ClientRpc]
		public void RpcDockCard(NetworkIdentity dockedTransform)
		{
			DockCard(dockedTransform.GetComponent<Card>());
		}


		private void ResetPositions()
		{
			Vector3 pos = transform.position;
			pos.y = origY;
			if (DockedCard != null)
			{
				DockedCard.transform.position = pos + Vector3.up * yCardOffset;
			}
			transform.position = pos;
		}

		// Update is called once per frame
		void Update()
		{
			if (!Initialized && CardNetworkManager.Instance.numPlayers == 2)
			{
				Initialized = true;
				RpcSetSocketSide(SocketSide);
			}
			if (!Active || DockedCard == null || !DockedCard.hasAuthority) return;
			Vector3 pos = transform.position;
			pos.y = origY + yOffset + Mathf.Sin(Time.realtimeSinceStartup * timeScale + timeOffset) * yScale;

			DockedCard.transform.position = pos + Vector3.up * yCardOffset;
			transform.position = pos;
		}
	}
}
