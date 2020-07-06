using System;
using UnityEngine;

namespace AwsomenautsCardGame.DataObjects.Networking {
	[Serializable]
	public class CardModelAsset
	{
		public GameObject RedTeam;
		public GameObject BlueTeam;
		public GameObject Get(int teamID) => teamID == 0 ? RedTeam : BlueTeam;
	}
}