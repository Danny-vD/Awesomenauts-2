using System;
using Enums.Deckbuilder;
using Interfaces;
using UnityEngine;

namespace Structs.Deckbuilder
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