using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Enums.Cards;
using Events.Deckbuilder;
using Structs.Deckbuilder;
using UI.Cards;
using UI.DeckBuilder;
using Utility;
using VDFramework.EventSystem;
using VDFramework.Extensions;

namespace Deckbuilder
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

		private readonly Dictionary<CardType, int> amountPerType = new Dictionary<CardType, int>();

		private List<AbstractUICard> collectionToCheck;

		public void Instantiate(List<AbstractUICard> collection)
		{
			collectionToCheck = collection;

			if (UI)
			{
				UI.SetRestrictions(minMaxPerCardTypes, maxSameCard);
			}

			PopulateDictionary();

			AddListeners();
		}

		public void Destroy()
		{
			RemoveListeners();
		}

		public void OnValidate()
		{
			FakeDictionaryUtil.PopulateEnumDictionary<MinMaxPerCardType, CardType, Vector2>(minMaxPerCardTypes);
		}

		private void AddListeners()
		{
			EventManager.Instance.AddListener<ClickUICardEvent>(OnClickUICard, int.MaxValue);
		}

		private void RemoveListeners()
		{
			if (!EventManager.IsInitialized)
			{
				return;
			}

			EventManager.Instance.RemoveListener<ClickUICardEvent>(OnClickUICard);
		}

		public void ScanDeck()
		{
			amountPerType.Clear();
			PopulateDictionary();

			foreach (AbstractUICard card in collectionToCheck)
			{
				ChangeAmountOfType(card.Type, card.Amount);
			}
		}

		public bool IsValid()
		{
			// Foreach CardType: get the minMax, check if our cached Amount is within that range, if not: return false
			if ((from pair in amountPerType
				let minMax = minMaxPerCardTypes.First(item => item.Key == pair.Key).Value
				where pair.Value < minMax.x || pair.Value > minMax.y
				select pair).Any())
			{
				return false;
			}

			return GetHighestCardAmount() <= maxSameCard;
		}

		private void PopulateDictionary()
		{
			foreach (CardType type in CardType.Action.GetValues())
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
			List<int> enumerable = collectionToCheck.Select(card => card.Amount).ToList();
			return !enumerable.Any() ? 0 : enumerable.Max();
		}

		private void OnClickUICard(ClickUICardEvent clickUICardEvent)
		{
			int amountToChange = 1;

			if (clickUICardEvent.CardIsInDeck)
			{
				amountToChange = -1;
			}

			ChangeAmountOfType(clickUICardEvent.Card.Type, amountToChange);

			if (UI)
			{
				UI.UpdateUI(amountPerType);
			}
		}
	}
}