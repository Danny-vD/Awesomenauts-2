using System.Collections.Generic;
using System.Linq;
using Deckbuilder;
using DeckBuilder.DeckFilterUtil;
using Enums.Deckbuilder;
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

		[SerializeField]
		private DeckRequirements deckRequirements = null;

		private readonly List<AbstractUICard> currentDeck = new List<AbstractUICard>();
		private List<AbstractUICard> availableCards;

		private DeckFilter deckFilter;
		private DeckSorter deckSorter;

		private void Start()
		{
			deckFilter = new DeckFilter();
			deckSorter = new DeckSorter();
			deckRequirements.Instantiate(currentDeck);

			availableCards = AddAllCards.AddCardsAsChild(availableCardsParent);
			EventManager.Instance.RaiseEvent(new HoverUICardEvent(availableCards[0]));

			AddDecksToBeFiltered();
			AddDecksToBeSorted();

			GetPeristentDeck();

			deckRequirements.ScanDeck();
			DeckSorter.SetSortings(SortValue.Type);
		}

		public void UpdateDictionaries()
		{
			deckRequirements.OnValidate();
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

			deckRequirements.Destroy();
			deckRequirements = null;
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
					clickedAvailableCard.Type, clickedAvailableCard.Awesomenaut, clickedAvailableCard.Filters);
				deckCard.Amount = 1;

				deckCard.CardAesthetics.Initialise(clickedAvailableCard.CardAesthetics);
				deckCard.name = clickedAvailableCard.name;

				DeckFilterManager.AddIsInDeckFilter(deckCard);
				DeckFilterManager.AddIsInDeckFilter(clickedAvailableCard);

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
						clickedCardInDeck.Type, clickedCardInDeck.Awesomenaut, clickedCardInDeck.Filters);
				availableCard.Amount = 1;

				availableCard.CardAesthetics.Initialise(clickedCardInDeck.CardAesthetics);
				availableCard.name = clickedCardInDeck.name;

				availableCards.Add(availableCard);
			}

			if (--clickedCardInDeck.Amount <= 0)
			{
				DeckFilterManager.AddIsNotInDeckFilter(availableCard);

				currentDeck.Remove(clickedCardInDeck);
				Destroy(clickedCardInDeck.gameObject);
			}
		}

		private bool GetCardFromDeck(AbstractUICard card, out AbstractUICard deckCard) =>
			GetCardFromCollection(currentDeck, card, out deckCard);

		private bool GetCardFromAvailableCards(AbstractUICard card, out AbstractUICard availableCard) =>
			GetCardFromCollection(availableCards, card, out availableCard);

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

			if (deckRequirements.IsValid())
			{
				EventManager.Instance.RaiseEvent(new SaveDeckEvent(ConvertDeckToIDlist()));
			}
		}
	}
}