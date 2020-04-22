using System.Collections.Generic;
using System.Linq;
using Enums.Deckbuilder;
using Events.Deckbuilder;
using Networking;
using UI.Cards;
using UnityEngine;
using Utility.UI;
using VDFramework;
using VDFramework.EventSystem;
using VDFramework.Utility;

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
		private DeckSorter deckSorter;

		private void Start()
		{
			deckFilter = new DeckFilter();
			deckSorter = new DeckSorter();

			availableCards = AddAllCards.AddCardsAsChild(availableCardsParent);

			AddDecksToBeFiltered();
			AddDecksToBeSorted();

			GetPeristentDeck();

			DeckSorter.SetSortings(SortValue.Type);
		}

		private void AddDecksToBeFiltered()
		{
			deckFilter.AddToFilter(currentDeck);
			deckFilter.AddToFilter(availableCards);
		}

		private void AddDecksToBeSorted()
		{
			deckSorter.AddToBeSorted(currentDeck);
			deckSorter.AddToBeSorted(availableCards);
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
			deckFilter = null;

			deckSorter.Destroy();
			deckSorter = null;
		}

		private void AddListeners()
		{
			EventManager.Instance.AddListener<ClickUICardEvent>(OnClickUICard);
			EventManager.Instance.AddListener<ToggleCurrentDeckFilterEvent>(OnToggleCurrentDeckFilter);
			EventManager.Instance.AddListener<ToggleAvailableCardsFilterEvent>(OnToggleAvailableCardsFilter);
		}

		private void RemoveListeners()
		{
			if (!EventManager.IsInitialized)
			{
				return;
			}

			EventManager.Instance.RemoveListener<ClickUICardEvent>(OnClickUICard);
			EventManager.Instance.RemoveListener<ToggleCurrentDeckFilterEvent>(OnToggleCurrentDeckFilter);
			EventManager.Instance.RemoveListener<ToggleAvailableCardsFilterEvent>(OnToggleAvailableCardsFilter);
		}

		private static bool GetCardFromCollection(IEnumerable<AbstractUICard> cards, AbstractUICard cardToFind,
			out AbstractUICard cardInCollection)
		{
			cardInCollection = cards.FirstOrDefault(item => item.Equals(cardToFind));

			return cardInCollection != null && cardInCollection.Amount > 0;
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
					clickedAvailableCard.Type, clickedAvailableCard.Filters);
				deckCard.Sprite = clickedAvailableCard.Sprite;
				deckCard.Amount = 1;

				DeckFilter.AddIsInDeckFilter(deckCard);
				DeckFilter.AddIsInDeckFilter(clickedAvailableCard);

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
						clickedCardInDeck.Type, clickedCardInDeck.Filters);
				availableCard.Sprite = clickedCardInDeck.Sprite;
				availableCard.Amount = 1;

				availableCards.Add(availableCard);
			}

			if (--clickedCardInDeck.Amount <= 0)
			{
				DeckFilter.AddIsNotInDeckFilter(availableCard);

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

		private void OnToggleCurrentDeckFilter(ToggleCurrentDeckFilterEvent toggleCurrentDeckFilterEvent)
		{
			if (toggleCurrentDeckFilterEvent.ShouldBeFiltered)
			{
				deckFilter.AddToFilter(currentDeck);
				return;
			}

			deckFilter.RemoveFromFilter(currentDeck);
		}

		private void OnToggleAvailableCardsFilter(ToggleAvailableCardsFilterEvent toggleAvailableCardsFilterEvent)
		{
			if (toggleAvailableCardsFilterEvent.ShouldBeFiltered)
			{
				deckFilter.AddToFilter(availableCards);
				return;
			}

			deckFilter.RemoveFromFilter(availableCards);
		}

		private void OnClickUICard(ClickUICardEvent clickUICardEvent)
		{
			AbstractUICard card = clickUICardEvent.Card;

			if (clickUICardEvent.CardIsInDeck)
			{
				if (!GetCardFromDeck(card, out _))
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