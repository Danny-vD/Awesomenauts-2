using System;
using Enums.Cards;
using Interfaces;
using UnityEngine;

namespace Structs.Deckbuilder
{
	[Serializable]
	public struct MinMaxPerCardType : IKeyValuePair<CardType, Vector2>
	{
		[SerializeField]
		private CardType cardType;
		
		[SerializeField]
		private Vector2 minMax;
		
		public CardType Key
		{
			get => cardType;
			set => cardType = value;
		}

		public Vector2 Value
		{
			get => minMax;
			set => minMax = value;
		}


		public bool Equals(IKeyValuePair<CardType, Vector2> other)
		{
			// ReSharper disable once PossibleNullReferenceException
			return Key.Equals(other.Key);
		}
	}
}