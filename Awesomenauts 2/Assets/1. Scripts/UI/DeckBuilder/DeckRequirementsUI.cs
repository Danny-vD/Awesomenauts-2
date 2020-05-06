using System;
using System.Collections.Generic;
using System.Linq;
using Enums.Cards;
using Structs.Deckbuilder;
using UnityEngine;
using Utility;

namespace UI.DeckBuilder
{
	[Serializable]
	public class DeckRequirementsUI
	{
		[SerializeField]
		private List<UIElementsPerCardType> UIelements = null;

		[SerializeField]
		private RequirementsUIElement cardCountUI = null;

		[SerializeField]
		private RequirementsUIElement totalCardCountUI = null;

		private List<MinMaxPerCardType> restrictions;
		private int maxAmountPerCard;
		private int maxTotalCards;

		public void Instantiate(List<MinMaxPerCardType> minMaxRestrictions, int maxCardAmount, int maxCardTotal)
		{
			restrictions = minMaxRestrictions;
			maxAmountPerCard = maxCardAmount;
			maxTotalCards = maxCardTotal;

			SetRestrictions();
		}

		public void Update(Dictionary<CardType, int> currentAmounts, int highestCardAmount, int totalCardCount)
		{
			CheckValidity(currentAmounts);
			IsValid(highestCardAmount, new Vector2Int(0, maxAmountPerCard), cardCountUI);
			IsValid(totalCardCount, new Vector2Int(0, maxTotalCards), totalCardCountUI);
		}

		public void OnValidate()
		{
			FakeDictionaryUtil.PopulateEnumDictionary<UIElementsPerCardType, CardType, List<RequirementsUIElement>>(
				UIelements);
		}

		private void SetRestrictions()
		{
			foreach (MinMaxPerCardType pair in restrictions)
			{
				foreach (RequirementsUIElement uiElement in GetUIElements(pair.Key))
				{
					if (!uiElement)
					{
						Debug.LogWarning($"A given UI element for {pair.Key} is not assigned!");
						continue;
					}
					
					uiElement.UpdateMinMax(pair.Value.x, pair.Value.y);
				}
			}

			if (cardCountUI)
			{
				cardCountUI.UpdateMinMax(0, maxAmountPerCard);
			}

			if (totalCardCountUI)
			{
				totalCardCountUI.UpdateMinMax(0, maxTotalCards);
			}
		}

		private void CheckValidity(Dictionary<CardType, int> currentAmounts)
		{
			foreach (KeyValuePair<CardType, int> pair in currentAmounts)
			{
				foreach (RequirementsUIElement uiElement in GetUIElements(pair.Key))
				{
					IsValid(pair.Value, GetRestrictions(pair.Key), uiElement);
				}
			}
		}

		private static void IsValid(int amount, Vector2Int minMax, RequirementsUIElement uiElement)
		{
			if (!uiElement)
			{
				return;
			}
			
			uiElement.SetValid(minMax.x <= amount && amount <= minMax.y);
			uiElement.UpdateAmount(amount);
		}

		private IEnumerable<RequirementsUIElement> GetUIElements(CardType type)
		{
			return UIelements.First(pair => pair.Key == type).Value;
		}

		private Vector2Int GetRestrictions(CardType type)
		{
			return restrictions.First(item => item.Key == type).Value;
		}
	}
}