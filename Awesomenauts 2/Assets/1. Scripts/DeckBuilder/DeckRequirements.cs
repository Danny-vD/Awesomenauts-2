using System;
using System.Collections.Generic;
using System.Linq;
using AwsomenautsCardGame.Enums.Cards;
using AwsomenautsCardGame.Events.Deckbuilder;
using AwsomenautsCardGame.Structs.Deckbuilder;
using AwsomenautsCardGame.UI.Cards;
using AwsomenautsCardGame.UI.DeckBuilder;
using AwsomenautsCardGame.Utility;
using VDFramework.SharedClasses.EventSystem;
using VDFramework.SharedClasses.Extensions;
using UnityEngine;

namespace AwsomenautsCardGame.DeckBuilder
{
	[Serializable]
	public class DeckRequirements
	{
		[SerializeField]
		private DeckRequirementsUI UI = null;

		[SerializeField]
		private List<MinMaxPerCardType> minMaxPerCardTypes = null;

		[SerializeField]
		private int maxSameCard = 3;

		[SerializeField]
		private int maxTotalCards = 25;

		private readonly Dictionary<CardType, int> amountPerType = new Dictionary<CardType, int>();

		private List<AbstractUICard> collectionToCheck;

		public void Instantiate(List<AbstractUICard> collection)
		{
			collectionToCheck = collection;

			UI.Instantiate(minMaxPerCardTypes, maxSameCard, maxTotalCards);

			PopulateDictionary();

			AddListeners();
		}

		public void Destroy()
		{
			UI = null;
			RemoveListeners();
		}

		public void OnValidate()
		{
			FakeDictionaryUtil.PopulateEnumDictionary<MinMaxPerCardType, CardType, Vector2Int>(minMaxPerCardTypes);
			UI.OnValidate();
		}

		private void AddListeners()
		{
			EventManager.Instance.AddListener<ClickUICardEvent>(OnClickUICard, int.MaxValue);
			EventManager.Instance.AddListener<ClickUICardEvent>(UpdateUI, int.MinValue);
		}

		private void RemoveListeners()
		{
			if (!EventManager.IsInitialized)
			{
				return;
			}

			EventManager.Instance.RemoveListener<ClickUICardEvent>(OnClickUICard);
			EventManager.Instance.RemoveListener<ClickUICardEvent>(UpdateUI);
		}

		public void ScanDeck()
		{
			amountPerType.Clear();
			PopulateDictionary();

			foreach (AbstractUICard card in collectionToCheck)
			{
				ChangeAmountOfType(card.Type, card.Amount);
			}
			
			UI.Update(amountPerType, GetHighestCardAmount(), GetTotalCardCount());
		}

		public bool IsValid()
		{
			if (GameInitializer.Instance.GameData.DebugInfo.AllowUnrestrictedDecks) return true;

			// Foreach CardType: get the minMax, check if our cached Amount is within that range, if not: return false
			foreach (KeyValuePair<CardType, int> pair in amountPerType)
			{
				IEnumerable<MinMaxPerCardType> entries = minMaxPerCardTypes.Where(item => item.Key == pair.Key);
				if (!entries.Any()) break;
				Vector2Int minMax = entries.First(item => item.Key == pair.Key).Value;
				if (pair.Value < minMax.x || pair.Value > minMax.y) return false;
			}

			if (GetHighestCardAmount() > maxSameCard)
			{
				return false;
			}

			return GetTotalCardCount() <= maxTotalCards;
		}

		private void PopulateDictionary()
		{
			foreach (CardType type in default(CardType).GetValues())
			{
				amountPerType.Add(type, 0);
			}
		}

		private void ChangeAmountOfType(CardType type, int amount)
		{
			amountPerType[type] += amount;
		}

		private int GetHighestCardAmount()
		{
			List<int> cardAmounts = collectionToCheck.Select(card => card.Amount).ToList();
			return cardAmounts.Any() ? cardAmounts.Max() : 0;
		}

		private int GetTotalCardCount()
		{
			return collectionToCheck.Aggregate(0, (amount, card) => amount + card.Amount);
		}

		private void OnClickUICard(ClickUICardEvent clickUICardEvent)
		{
			int amountToChange = 1;

			if (clickUICardEvent.CardIsInDeck)
			{
				amountToChange = -1;
			}

			ChangeAmountOfType(clickUICardEvent.Card.Type, amountToChange);
		}

		private void UpdateUI()
		{
			UI.Update(amountPerType, GetHighestCardAmount(), GetTotalCardCount());
		}
	}
}