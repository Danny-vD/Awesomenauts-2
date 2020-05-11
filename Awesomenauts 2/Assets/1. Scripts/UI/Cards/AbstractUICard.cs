using System;
using DeckBuilder.DeckFilterUtil;
using Enums.Cards;
using Enums.Character;
using Enums.Deckbuilder;
using Events.Deckbuilder;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VDFramework;
using VDFramework.EventSystem;
using VDFramework.UnityExtensions;

namespace UI.Cards
{
	[DisallowMultipleComponent]
	public abstract class AbstractUICard : BetterMonoBehaviour, IEquatable<AbstractUICard>
	{
		public int ID { get; set; }

		private UICardAesthetics cardAesthetics = null;

		public UICardAesthetics CardAesthetics
		{
			get
			{
				if (!cardAesthetics)
				{
					cardAesthetics = GetComponentInChildren<UICardAesthetics>();
				}
				
				return cardAesthetics;
			}
		}

		private int amount = 1;

		public int Amount
		{
			get => amount;
			set => amountCounter.Amount = amount = value;
		}

		public Sprite Sprite
		{
			get => image.sprite;
			set => SetSprite(value);
		}

		public FilterValues Filters { get; set; } = 0;

		private CardType type;

		public CardType Type
		{
			get => type;
			set
			{
				SetCardTypeFilter(value);
				type = value;
			}
		}

		public Awesomenaut Awesomenaut { get; set; }

		private Image image;
		private Button button;

		private CardAmountCounter amountCounter;

		protected virtual void Awake()
		{
			button = GetComponent<Button>();
			button.onClick.AddListener(OnPointerClick);

			image = GetComponent<Image>();

			AddEventTriggers();

			amountCounter = GetComponentInChildren<CardAmountCounter>();
		}

		public bool MeetsFilters(FilterValues currentFilters)
		{
			return (Filters & currentFilters) != 0;
		}

		public bool MeetsFilters(Awesomenaut awesomenautFilters)
		{
			return (Awesomenaut & awesomenautFilters) != 0;
		}

		public bool MeetsFilters(FilterValues currentFilters, Awesomenaut awesomenautFilters)
		{
			return (Filters & currentFilters) != 0 && (Awesomenaut & awesomenautFilters) != 0;
		}

		protected virtual void OnPointerEnter()
		{
			EventManager.Instance.RaiseEvent(new HoverUICardEvent(this));
		}

		protected abstract void OnPointerClick();

		protected virtual void OnPointerExit() { }

		private void SetSprite(Sprite sprite)
		{
			if (!image)
			{
				image = GetComponent<Image>();
			}

			image.sprite = sprite;
			gameObject.name = sprite.name;
		}

		private void AddEventTriggers()
		{
			EventTrigger.Entry pointerEnterEntry = new EventTrigger.Entry {eventID = EventTriggerType.PointerEnter};
			pointerEnterEntry.callback.AddListener((data) => { OnPointerEnter(); });

			EventTrigger.Entry pointerExitEntry = new EventTrigger.Entry {eventID = EventTriggerType.PointerExit};
			pointerExitEntry.callback.AddListener((data) => { OnPointerExit(); });

			EventTrigger trigger = gameObject.EnsureComponent<EventTrigger>();
			trigger.triggers.Add(pointerEnterEntry);
			trigger.triggers.Add(pointerExitEntry);
		}

		// Used by List.Contains()
		public bool Equals(AbstractUICard other)
		{
			if (other == null)
			{
				return this == null;
			}

			return ID == other.ID;
		}

		private void SetCardTypeFilter(CardType newType)
		{
			DeckFilterManager.RemoveFilterFlagFromCard(this, (FilterValues) type);
			DeckFilterManager.AddFilterFlagToCard(this, (FilterValues) newType);
		}
	}
}