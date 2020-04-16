using System;
using Enums.Deckbuilder;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VDFramework;
using VDFramework.Extensions;
using VDFramework.UnityExtensions;

namespace UI.Cards
{
	[DisallowMultipleComponent]
	public abstract class AbstractUICard : BetterMonoBehaviour, IEquatable<AbstractUICard>
	{
		public int ID { get; set; }

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

		public FilterValues Filters { get; set; } = FilterValues.Owned;

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

		protected virtual void OnPointerEnter() { }

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

		public bool MeetsFilters(FilterValues currentFilters)
		{
			return ((int) Filters).HasOneMatchingBit((int) currentFilters);
		}
	}
}