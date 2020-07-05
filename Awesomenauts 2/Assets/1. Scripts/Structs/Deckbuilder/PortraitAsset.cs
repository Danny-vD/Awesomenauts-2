using System;
using Networking;
using UnityEngine;

namespace Structs.Deckbuilder {
	[Serializable]
	public class PortraitAsset : TeamAsset<Sprite>
	{
		public static implicit operator PortraitAsset(Sprite spr) => new PortraitAsset() {RedTeam = spr, BlueTeam = spr};
	}
}