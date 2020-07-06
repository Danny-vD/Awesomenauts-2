using System;
using UnityEngine;

namespace AwsomenautsCardGame.Structs.Deckbuilder
{
	[Serializable]
	public struct CardSprites
	{
		public Sprite Border;

		public Sprite Body;
		
		public PortraitAsset TeamPortrait;
	}
}