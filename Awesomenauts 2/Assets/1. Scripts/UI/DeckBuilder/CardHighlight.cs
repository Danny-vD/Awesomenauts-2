using Enums.Deckbuilder;
using Events.Deckbuilder;
using UI.Cards;
using UnityEngine;
using VDFramework;
using VDFramework.EventSystem;

namespace UI.DeckBuilder
{
	public class CardHighlight : BetterMonoBehaviour
	{
		private AbstractUICard card;

		[SerializeField, HideInInspector]
		private GameObject highlight = null;

		[SerializeField, HideInInspector]
		private GameObject darkOverlay = null;

		[SerializeField]
		private FilterValues highlightValues = FilterValues.IsInDeck;

		private void Start()
		{
			card = GetComponent<AbstractUICard>();
			EnableHighlight(card.MeetsFilters(highlightValues));
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
			EventManager.Instance.AddListener<HoverUICardEvent>(OnHoverUICard);
		}

		private void RemoveListeners()
		{
			if (!EventManager.IsInitialized)
			{
				return;
			}

			EventManager.Instance.RemoveListener<HoverUICardEvent>(OnHoverUICard);
		}

		private void OnHoverUICard(HoverUICardEvent hoverUICardEvent)
		{
			if (hoverUICardEvent.Card.Equals(card))
			{
				EnableHighlight(true);
				return;
			}

			EnableHighlight(card && card.MeetsFilters(highlightValues));
		}

		private void EnableHighlight(bool enable)
		{
			highlight.SetActive(enable);
			darkOverlay.SetActive(!enable);
		}
	}
}