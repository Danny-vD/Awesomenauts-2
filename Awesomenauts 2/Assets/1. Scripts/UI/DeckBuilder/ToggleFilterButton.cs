using System;
using Events.Deckbuilder;
using UnityEngine;
using UnityEngine.UI;
using VDFramework;
using VDFramework.EventSystem;

namespace UI.DeckBuilder
{
	[RequireComponent(typeof(Image), typeof(Button))]
	public class ToggleFilterButton : BetterMonoBehaviour
	{
		private enum ToggleFilterFor
		{
			CurrentDeck,
			AvailableCards,
		}

		private Sprite Sprite
		{
			set => image.sprite = value;
		}

		[SerializeField]
		private ToggleFilterFor toggleFilterFor = ToggleFilterFor.CurrentDeck;

		[SerializeField]
		private Sprite unlockSprite = null;

		[SerializeField]
		private Sprite lockSprite = null;

		private Image image;
		private Button button;

		private bool shouldBeFiltered = true;

		private Action toggleCollectionFilter = null;

		private void Awake()
		{
			image = GetComponent<Image>();
			button = GetComponent<Button>();

			Sprite = unlockSprite;

			// ReSharper disable once ConvertSwitchStatementToSwitchExpression
			switch (toggleFilterFor)
			{
				case ToggleFilterFor.CurrentDeck:
					toggleCollectionFilter = ToggleCurrentDeckFilter;
					break;
				case ToggleFilterFor.AvailableCards:
					toggleCollectionFilter = ToggleAvailableCardsFilter;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			button.onClick.AddListener(ToggleFilter);
		}

		private void ToggleFilter()
		{
			shouldBeFiltered = !shouldBeFiltered;

			Sprite = shouldBeFiltered ? unlockSprite : lockSprite;

			toggleCollectionFilter();
		}

		private void ToggleCurrentDeckFilter()
		{
			EventManager.Instance.RaiseEvent(new ToggleCurrentDeckFilterEvent(shouldBeFiltered));
		}

		private void ToggleAvailableCardsFilter()
		{
			EventManager.Instance.RaiseEvent(new ToggleAvailableCardsFilterEvent(shouldBeFiltered));
		}
	}
}