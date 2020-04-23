using System.Collections.Generic;
using System.Linq;
using Enums.Cards;
using Structs.Deckbuilder;
using UnityEngine;
using VDFramework;

namespace UI.DeckBuilder
{
	public class DeckRequirementsUI : BetterMonoBehaviour
	{
		private List<MinMaxPerCardType> restrictions;
		private int maxAmountPerCard;

		public void UpdateUI(Dictionary<CardType, int> currentAmounts)
		{
			CheckValidity(currentAmounts);
		}

		public void SetRestrictions(List<MinMaxPerCardType> minMaxRestrictions, int maxCardAmount)
		{
			restrictions = minMaxRestrictions;
			maxAmountPerCard = maxCardAmount;
		}
		
		private void CheckValidity(Dictionary<CardType, int> currentAmounts)
		{
			foreach (KeyValuePair<CardType, int> pair in currentAmounts)
			{
				if (!IsValid(pair.Key, pair.Value))
				{
					//Set text normal, disable game object
					print("pass");
					continue;
				}
				
				//Set text to fail, enable game object
				print("fail");
			}
		}

		private bool IsValid(CardType type, int amount)
		{
			Vector2 minMax = restrictions.First(item => item.Key == type).Value;

			return amount >= minMax.x && amount <= minMax.y;
		}
	}
}