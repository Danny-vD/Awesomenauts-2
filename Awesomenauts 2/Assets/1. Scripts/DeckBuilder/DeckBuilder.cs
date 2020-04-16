using System;
using System.Collections.Generic;
using System.Linq;
using Events.Deckbuilder;
using Networking;
using UI.Cards;
using UnityEngine;
using Utility.UI;
using VDFramework;
using VDFramework.EventSystem;

namespace DeckBuilder
{
	public class DeckBuilder : BetterMonoBehaviour
	{
		[SerializeField]
		private Transform currentDeckParent = null;

		[SerializeField]
		private Transform availableCardsParent = null;

		private readonly List<AbstractUICard> currentDeck = new List<AbstractUICard>();
		private List<AbstractUICard> availableCards;

		private DeckFilter deckFilter;

		private void Start()
		{
			deckFilter = new DeckFilter();

			availableCards = AddAllCards.AddCardsAsChild(availableCardsParent);

			AddDecksToBeFiltered();

			GetOwnedCards();

			GetPeristentDeck();
		}

		private void AddDecksToBeFiltered()
		{
			deckFilter.AddToFilter(currentDeck);
			deckFilter.AddToFilter(availableCards);
		}

		private void OnEnable()
		{
			AddListeners();
		}

		private void OnDisable()
		{
			RemoveListeners();
		}

		private void OnDestroy()
		{
			deckFilter.Destroy();
		}

		private void AddListeners()
		{
			EventManager.Instance.AddListener<ClickUICardEvent>(OnClickUICard);
		}

		private void RemoveListeners()
		{
			if (EventManager.IsInitialized)
			{
				EventManager.Instance.RemoveListener<ClickUICardEvent>(OnClickUICard);
			}
		}

		private void GetOwnedCards()
		{
			//HACK: get actual owned cards eventually
			availableCards.ForEach(card => card.Amount = 3);
		}

		private void GetPeristentDeck()
		{
			int[] deckIDs = CardNetworkManager.Instance.CardsInDeck;

			deckIDs.Select(id => availableCards.First(availableCard => availableCard.ID == id)).ToList()
				.ForEach(AddToDeck);
		}

		private void AddToDeck(AbstractUICard clickedAvailableCard)
		{
			if (GetCardFromDeck(clickedAvailableCard, out AbstractUICard deckCard))
			{
				++deckCard.Amount;
			}
			else
			{
				deckCard = UICardFactory.Instance.CreateNewCard<DeckUICard>(currentDeckParent, clickedAvailableCard.ID,
					clickedAvailableCard.Filters);
				deckCard.Sprite = clickedAvailableCard.Sprite;

				currentDeck.Add(deckCard);
			}

			if (--clickedAvailableCard.Amount <= 0)
			{
				availableCards.Remove(clickedAvailableCard);
				Destroy(clickedAvailableCard.gameObject);
			}
		}

		private void RemoveFromDeck(AbstractUICard clickedCardInDeck)
		{
			if (GetCardFromAvailableCards(clickedCardInDeck, out AbstractUICard availableCard))
			{
				++availableCard.Amount;
			}
			else
			{
				availableCard =
					UICardFactory.Instance.CreateNewCard<AvailableUICard>(availableCardsParent, clickedCardInDeck.ID,
						clickedCardInDeck.Filters);
				availableCard.Sprite = clickedCardInDeck.Sprite;

				availableCards.Add(availableCard);
			}

			if (--clickedCardInDeck.Amount <= 0)
			{
				currentDeck.Remove(clickedCardInDeck);
				Destroy(clickedCardInDeck.gameObject);
			}
		}

		private bool GetCardFromDeck(AbstractUICard card, out AbstractUICard deckCard)
		{
			return GetCardFromCollection(currentDeck, card, out deckCard);
		}

		private bool GetCardFromAvailableCards(AbstractUICard card, out AbstractUICard availableCard)
		{
			return GetCardFromCollection(availableCards, card, out availableCard);
		}

		private static bool GetCardFromCollection(IEnumerable<AbstractUICard> cards, AbstractUICard cardToFind,
			out AbstractUICard cardInCollection)
		{
			cardInCollection = cards.FirstOrDefault(item => item.Equals(cardToFind));

			return cardInCollection != null && cardInCollection.Amount > 0;
		}

		private IEnumerable<int> ConvertDeckToIDlist()
		{
			List<int> ids = new List<int>();

			foreach (AbstractUICard card in currentDeck)
			{
				for (int i = 0; i < card.Amount; i++)
				{
					ids.Add(card.ID);
				}
			}

			return ids;
		}

		private void OnClickUICard(ClickUICardEvent clickUICardEvent)
		{
			AbstractUICard card = clickUICardEvent.Card;

			if (clickUICardEvent.CardIsInDeck)
			{
				if (!GetCardFromDeck(card, out AbstractUICard _))
				{
					Debug.LogError($"{card} is not present in the current deck");
					return;
				}

				RemoveFromDeck(card);
			}
			else
			{
				if (!GetCardFromAvailableCards(card, out _))
				{
					Debug.LogError($"{card} is not present in the available cards");
					return;
				}

				AddToDeck(card);
			}

			EventManager.Instance.RaiseEvent(new SaveDeckEvent(ConvertDeckToIDlist()));
		}
	}
}