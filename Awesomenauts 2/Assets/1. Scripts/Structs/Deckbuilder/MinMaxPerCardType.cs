using System;
using AwsomenautsCardGame.Enums.Cards;
using AwsomenautsCardGame.Interfaces;
using UnityEngine;

namespace AwsomenautsCardGame.Structs.Deckbuilder
{
	[Serializable]
	public struct MinMaxPerCardType : IKeyValuePair<CardType, Vector2Int>
	{
		[SerializeField]
		private CardType cardType;
		
		[SerializeField]
		private Vector2Int minMax;
		
		public CardType Key
		{
			get => cardType;
			set => cardType = value;
		}

		public Vector2Int Value
		{
			get => minMax;
			set => minMax = value;
		}


		public bool Equals(IKeyValuePair<CardType, Vector2Int> other)
		{
			// ReSharper disable once PossibleNullReferenceException
			return Key.Equals(other.Key);
		}
	}
}