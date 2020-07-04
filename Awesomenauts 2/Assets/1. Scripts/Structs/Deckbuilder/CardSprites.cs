using System;
using Networking;
using UnityEngine;

namespace Structs.Deckbuilder
{
	[Serializable]
	public class PortraitAsset : TeamAsset<Sprite> { }

	[Serializable]
	public struct CardSprites
	{
		public Sprite Border;

		public Sprite Body;

		public Sprite Portrait;
		public PortraitAsset TeamPortrait;
	}
}