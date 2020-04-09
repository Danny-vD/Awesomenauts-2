using System;
using UnityEngine;

namespace Maps {
	public class MapTransformInfo : MonoBehaviour
	{
		/// <summary>
		/// Contains the Info for the Initialization of the Player Prefab
		/// </summary>
		[Serializable]
		public struct PlayerTransformInfo
		{
			public Transform DeckPosition;
			public Transform HandPosition;
			public Transform CameraPosition;
			public Transform GravePosition;
		}

		/// <summary>
		/// Contains the Transform Information for all teams/players
		/// </summary>
		public PlayerTransformInfo[] PlayerTransformInfos;

		/// <summary>
		/// Reference to the Socket Manager
		/// </summary>
		public SocketManager SocketManager;


		public static MapTransformInfo Instance;
		// Start is called before the first frame update
		void Start()
		{
			Instance = this;
		}

		// Update is called once per frame
		void Update()
		{
        
		}
	}
}
