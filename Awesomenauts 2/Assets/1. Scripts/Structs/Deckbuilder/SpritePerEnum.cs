using System;
using AwsomenautsCardGame.Enums.Deckbuilder;
using AwsomenautsCardGame.Interfaces;
using UnityEngine;

namespace AwsomenautsCardGame.Structs.Deckbuilder
{
	[Serializable]
	public struct SpritePerEnum : IKeyValuePair<FilterValues, Sprite>
	{
		[SerializeField]
		private FilterValues key;
		
		[SerializeField]
		private Sprite value;

		public FilterValues Key
		{
			get => key;
			set => key = value;
		}

		public Sprite Value
		{
			get => value;
			set => this.value = value;
		}

		public bool Equals(IKeyValuePair<FilterValues, Sprite> other)
		{
			throw new System.NotImplementedException();
		}
	}
}