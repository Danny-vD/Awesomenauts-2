using System;
using AwsomenautsCardGame.Networking;
using UnityEngine;

namespace AwsomenautsCardGame.Structs.Deckbuilder {
	[Serializable]
	public class PortraitAsset : TeamAsset<Sprite>
	{
		public static implicit operator PortraitAsset(Sprite spr) => new PortraitAsset() {RedTeam = spr, BlueTeam = spr};
	}
}