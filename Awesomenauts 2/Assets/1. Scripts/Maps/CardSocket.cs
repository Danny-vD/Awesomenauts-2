using Mirror;
using Networking;
using Player;
using UnityEngine;

namespace Maps
{
	public class CardSocket : NetworkBehaviour
	{
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
			DockedCard?.SetSocket(this);

			ResetPositions();
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
