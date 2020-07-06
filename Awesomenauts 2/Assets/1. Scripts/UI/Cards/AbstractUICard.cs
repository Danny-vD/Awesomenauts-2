using System;
using AwsomenautsCardGame.DeckBuilder.DeckFilterUtil;
using AwsomenautsCardGame.Enums.Cards;
using AwsomenautsCardGame.Enums.Character;
using AwsomenautsCardGame.Enums.Deckbuilder;
using AwsomenautsCardGame.Events.Deckbuilder;
using VDFramework.SharedClasses.EventSystem;
using VDFramework.VDUnityFramework.BaseClasses;
using VDFramework.VDUnityFramework.UnityExtensions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AwsomenautsCardGame.UI.Cards
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

		private Button button;

		private CardAmountCounter amountCounter;

		protected virtual void Awake()
		{
			button = GetComponent<Button>();
			button.onClick.AddListener(OnPointerClick);

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