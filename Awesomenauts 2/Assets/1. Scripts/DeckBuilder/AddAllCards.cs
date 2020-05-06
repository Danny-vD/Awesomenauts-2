using System.Collections.Generic;
using DeckBuilder.DeckFilterUtil;
using Enums.Deckbuilder;
using Networking;
using UI.Cards;
using UnityEngine;
using Utility.UI;

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

			AbstractUICard card =
				UICardFactory.Instance.CreateNewCard<AvailableUICard>(parent, id, entry.CardType, entry.Awesomenaut, FilterValues.IsNotInDeck);

			int amount = entry.Amount;
			card.Amount = amount;

			if (amount > 0)
			{
				DeckFilterManager.AddFilterFlagToCard(card, FilterValues.Owned);
			}

			card.Sprite = entry.cardSprite;

			return card;
		}
	}
}