using System;
using System.Collections.Generic;
using Enums.Deckbuilder;
using Events.Deckbuilder;
using UI.Cards;
using Object = UnityEngine.Object;

namespace DeckBuilder.DeckSorterUtil
{
	public class DeckSorterMethods
	{
		private static readonly Dictionary<SortValue, Comparison<AbstractUICard>> comparisons =
			new Dictionary<SortValue, Comparison<AbstractUICard>>()
			{
				{SortValue.IsInDeck, SortByIsInDeck},
				{SortValue.Name, SortByName},
				{SortValue.Amount, SortByAmount},
			};

		private List<SortValue> sortings = new List<SortValue>() {SortValue.Name};

		public DeckSorterMethods()
		{
			sortings.Sort();
			AddListeners();
		}

		public void Destroy()
		{
			RemoveListeners();
		}

		private void AddListeners()
		{
			VDFramework.EventSystem.EventManager.Instance.AddListener<SortingsChangedEvent>(OnSortingsChanged);
		}

		private void RemoveListeners()
		{
			if (!VDFramework.EventSystem.EventManager.IsInitialized)
			{
				return;
			}

			VDFramework.EventSystem.EventManager.Instance.RemoveListener<SortingsChangedEvent>(OnSortingsChanged);
		}

		public int Sort(AbstractUICard card, AbstractUICard other)
		{
			int compare = 0;

			foreach (SortValue sortValue in sortings)
			{
				compare = comparisons[sortValue](card, other);

				if (compare != 0)
				{
					return compare;
				}
			}

			return compare;
		}

		//////////////////////////////////
		/// 	Sorting methods
		//////////////////////////////////
		private static int SortByIsInDeck(AbstractUICard card, AbstractUICard other)
		{
			bool isInDeck = card.MeetsFilters(FilterValues.IsIndeck);
			bool otherIsInDeck = other.MeetsFilters(FilterValues.IsIndeck);

			if (isInDeck && !otherIsInDeck)
			{
				return -1;
			}

			if (!isInDeck && otherIsInDeck)
			{
				return 1;
			}

			return 0;
		}

		private static int SortByName(Object card, Object other)
		{
			return string.Compare(card.name, other.name, StringComparison.InvariantCultureIgnoreCase);
		}

		private static int SortByAmount(AbstractUICard card, AbstractUICard other)
		{
			int amount = card.Amount;
			int otherAmount = other.Amount;

			if (amount > otherAmount)
			{
				return -1;
			}

			if (amount < otherAmount)
			{
				return 1;
			}

			return 0;
		}

		//////////////////////////////////
		/// 	EventHandlers
		//////////////////////////////////
		private void OnSortingsChanged(SortingsChangedEvent sortingsChangedEvent)
		{
			sortings = sortingsChangedEvent.Sortings;

			sortings.Sort();
		}
	}
}