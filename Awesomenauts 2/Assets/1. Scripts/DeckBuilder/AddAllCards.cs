using System.Collections.Generic;
using DeckBuilder.DeckFilterUtil;
using Enums.Deckbuilder;
using Events.Deckbuilder;
using Networking;
using Player;
using UI.Cards;
using UnityEngine;
using Utility.UI;
using VDFramework.EventSystem;

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

				if (entry.Sprites.TeamPortrait.Get(0) == null)
				{
					entry.Sprites.TeamPortrait = CardNetworkManager.Instance.DefaultCardPortrait;
				}
				
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
			AbstractUICard card =
				UICardFactory.Instance.CreateNewCard<AvailableUICard>(parent, id, entry.CardType, entry.Awesomenaut, FilterValues.IsNotInDeck);

			int amount = entry.Amount;
			card.Amount = amount;
			card.CardAesthetics.Initialise(card, entry);

			card.name = entry.Statistics.GetValue(CardPlayerStatType.CardName).ToString();

			if (amount > 0)
			{
				DeckFilterManager.AddFilterFlagToCard(card, FilterValues.Owned);
			}

			return card;
		}
	}
}