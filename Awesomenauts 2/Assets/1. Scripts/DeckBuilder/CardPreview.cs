using Enums.Cards;
using Events.Deckbuilder;
using UI.Cards;
using UnityEngine;
using UnityEngine.UI;
using VDFramework;
using VDFramework.EventSystem;

namespace DeckBuilder
{
	public class CardPreview : BetterMonoBehaviour
	{
		private Text text;
		private UICardAesthetics cardAesthetics;
		private GameObject icons;

		private void Awake()
		{
			text = GetComponentInChildren<Text>();
			cardAesthetics = GetComponentInChildren<UICardAesthetics>();
			icons = cardAesthetics.CachedTransform.Find("Icons").gameObject;
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
			if (EventManager.IsInitialized)
			{
				EventManager.Instance.RemoveListener<HoverUICardEvent>(OnHoverUICard);
			}
		}

		private void OnHoverUICard(HoverUICardEvent hoverUICardEvent)
		{
			SetPreview(hoverUICardEvent.Card);
		}

		private void SetPreview(AbstractUICard card)
		{
			cardAesthetics.Initialise(card.CardAesthetics);

			text.text = card.name;

			icons.SetActive(card.Type != CardType.Action);
		}
	}
}