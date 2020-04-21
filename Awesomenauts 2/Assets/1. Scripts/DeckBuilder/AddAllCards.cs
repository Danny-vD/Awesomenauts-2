using System.Collections.Generic;
using Enums.Deckbuilder;
using Networking;
using UI.Cards;
using UnityEngine;
using Utility.UI;
using VDFramework.Utility;

namespace DeckBuilder
{
	public static class AddAllCards
	{
		public static List<AbstractUICard> AddCardsAsChild(Transform parentTransform)
		{
			List<AbstractUICard> cards = new List<AbstractUICard>();

			int length = CardNetworkManager.Instance.CardEntries.Length;

			for (int id = 0; id < length; id++)
			{
				CardEntry entry = CardNetworkManager.Instance.CardEntries[id];
				AbstractUICard card = InstantiateCardEntry(entry, parentTransform, id);

				if (card)
				{
					cards.Add(card);
				}
			}

			return cards;
		}

		private static AbstractUICard InstantiateCardEntry(CardEntry entry, Transform parent, int id)
		{
			if (entry.cardSprite == null)
			{
				return null;
			}

			//TODO: provide it with entry.filterValues;
			AbstractUICard card = UICardFactory.Instance.CreateNewCard<AvailableUICard>(parent, id, FilterValues.IsNotInDeck);
			DeckFilter.AddFilterFlagToCard(card, FilterValues.IsNotInDeck);

			//TODO: set amount to entry.Amount
			int amount = RandomUtil.GetRandom(0, 1, 2, 4, 5);
			card.Amount = amount;

			if (amount > 0)
			{
				DeckFilter.AddFilterFlagToCard(card, FilterValues.Owned);
			}
			
			card.Sprite = entry.cardSprite;

			return card;
		}
	}
}