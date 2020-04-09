using System;
using System.Collections.Generic;
using System.Linq;
using Events.Deckbuilder;
using UnityEngine;
using Utility.UI;
using VDFramework;
using VDFramework.EventSystem;

namespace DeckBuilder
{
	public class DeckBuilder : BetterMonoBehaviour
	{
		private List<AbstractUICard> currentDeck = new List<AbstractUICard>();

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

		private void OnClickUICard(ClickUICardEvent clickUICardEvent)
		{
			AbstractUICard card = clickUICardEvent.Card;

			if (clickUICardEvent.CardIsInDeck)
			{
				RemoveFromDeck(card);
			}
			else
			{
				AddToDeck(card);
			}
		}

		private void AddToDeck(AbstractUICard card)
		{
			if (GetCardFromDeck(card, out AbstractUICard deckCard))
			{
				++deckCard.Amount;
				return;
			}

			deckCard = UICardFactory.Instance.CreateNewCard<DeckUICard>(CachedTransform, card.ID);
			deckCard.Sprite = card.Sprite;
			
			currentDeck.Add(deckCard);
		}

		private void RemoveFromDeck(AbstractUICard card)
		{
			if (GetCardFromDeck(card, out AbstractUICard deckCard))
			{
				--deckCard.Amount;
				
				//TODO: increase the amount in the available cards
				
				if (deckCard.Amount != 0)
				{
					return;
				}

				currentDeck.Remove(deckCard);
				Destroy(deckCard.gameObject);

				return;
			}
			
			Debug.LogError($"{card} is not present in the current deck", this);
		}

		private bool GetCardFromDeck(AbstractUICard card, out AbstractUICard deckCard)
		{
			deckCard = currentDeck.FirstOrDefault(item => item.Equals(card));

			return deckCard != null && deckCard.Amount > 0;
		}
	}
}