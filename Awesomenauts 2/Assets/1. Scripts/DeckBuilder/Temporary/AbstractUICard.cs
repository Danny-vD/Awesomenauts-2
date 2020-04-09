using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VDFramework;
using VDFramework.UnityExtensions;

namespace DeckBuilder
{
	[DisallowMultipleComponent]
	public abstract class AbstractUICard : BetterMonoBehaviour, IEquatable<AbstractUICard>
	{
		public int ID { get; set; }
		public uint Amount { get; set; }

		public Sprite Sprite
		{
			get => image.sprite;
			set => SetSprite(value);
		}

		private Image image;
		private Button button;

		protected virtual void Start()
		{
			button = GetComponent<Button>();
			button.onClick.AddListener(OnPointerClick);

			image = GetComponent<Image>();

			AddEventTrigger();
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
		
		private void AddEventTrigger()
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
	}
}