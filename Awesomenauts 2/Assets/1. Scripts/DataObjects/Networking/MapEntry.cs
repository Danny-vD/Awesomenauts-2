using System;
using UnityEngine;

namespace AwsomenautsCardGame.DataObjects.Networking {
	/// <summary>
	/// Info Object for a Map
	/// </summary>
	[Serializable]
	public struct MapEntry
	{
		public GameObject Prefab;
		public Sprite MapIcon;
	}
}