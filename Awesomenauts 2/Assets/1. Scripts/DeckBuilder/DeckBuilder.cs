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
		private Transform currentDeckParent;

		[SerializeField]
		private Transform availableCardsParent;

		private readonly List<AbstractUICard> currentDeck = new List<AbstractUICard>();
		private List<AbstractUICard> availableCards;

		private void Start()
		{
			availableCards = AddAllCards.AddCardsAsChild(availableCardsParent);

			foreach (AbstractUICard card in availableCards)
			{
				card.Amount = 3;
			}


			int[] deckIDs = CardNetworkManager.Instance.CardsInDeck;

			deckIDs.Select(x => availableCards.First(y => y.ID == x)).ToList().ForEach(AddToDeck);

		}

		private void OnEnable()
		{
			AddListeners();
		}

		private void OnDisable()
		{
			RemoveListeners();
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

		private void AddToDeck(AbstractUICard clickedAvailableCard)
		{
			if (GetCardFromDeck(clickedAvailableCard, out AbstractUICard deckCard))
			{
				++deckCard.Amount;
			}
			else
			{
				deckCard = UICardFactory.Instance.CreateNewCard<DeckUICard>(currentDeckParent, clickedAvailableCard.ID);
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
					UICardFactory.Instance.CreateNewCard<AvailableUICard>(availableCardsParent, clickedCardInDeck.ID);
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