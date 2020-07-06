using System;
using System.Collections.Generic;
using AwsomenautsCardGame.Enums.Cards;
using AwsomenautsCardGame.Interfaces;
using AwsomenautsCardGame.UI.DeckBuilder;
using UnityEngine;

namespace AwsomenautsCardGame.Structs.Deckbuilder
{
	[Serializable]
	public struct UIElementsPerCardType : IKeyValuePair<CardType, List<RequirementsUIElement>>
	{
		[SerializeField]
		private CardType key;
		
		[SerializeField]
		private List<RequirementsUIElement> value;

		public CardType Key
		{
			get => key;
			set => key = value;
		}

		public List<RequirementsUIElement> Value
		{
			get => value;
			set => this.value = value;
		}

		public bool Equals(IKeyValuePair<CardType, List<RequirementsUIElement>> other)
		{
			if (other == null)
			{
				return false;
			}

			return key == other.Key;
		}
	}
}