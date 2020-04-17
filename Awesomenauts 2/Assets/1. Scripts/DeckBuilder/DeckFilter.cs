using System.Collections.Generic;
using System.Linq;
using Enums.Deckbuilder;
using Events.Deckbuilder;
using UI.Cards;
using VDFramework.EventSystem;

namespace DeckBuilder
{
	public class DeckFilter
	{
		private FilterValues currentFilters = FilterValues.ShowAll;

		private readonly List<IEnumerable<AbstractUICard>>
			collectionsToFilter = new List<IEnumerable<AbstractUICard>>();

		public DeckFilter()
		{
			AddListeners();
		}

		public void Destroy()
		{
			RemoveListeners();
		}

		private void AddListeners()
		{
			EventManager.Instance.AddListener<FiltersChangedEvent>(OnFiltersChanged);
			EventManager.Instance.AddListener<ClickUICardEvent>(OnClickUICard, int.MinValue);
			EventManager.Instance.AddListener<ToggleCurrentDeckFilterEvent>(OnToggleCurrentDeckFilter, int.MinValue);
			EventManager.Instance.AddListener<ToggleAvailableCardsFilterEvent>(OnToggleAvailableCardsFilter, int.MinValue);
		}

		private void RemoveListeners()
		{
			if (!EventManager.IsInitialized)
			{
				return;
			}
    
			EventManager.Instance.RemoveListener<ClickUICardEvent>(OnClickUICard);
		}

		public static void SetFilters(params FilterValues[] filters)
		{
			FilterValues values = filters.Aggregate<FilterValues, FilterValues>(0, (current, filterValue) => current | filterValue);
			
			EventManager.Instance.RaiseEvent(new FiltersChangedEvent(values));
		}

		public static void AddFilterFlagToCard(AbstractUICard card, FilterValues flagToAdd)
		{
			card.Filters |= flagToAdd;
		}

		public static void RemoveFilterFlagFromCard(AbstractUICard card, FilterValues flagToRemove)
		{
			card.Filters &= ~flagToRemove;
		}

		public void AddToFilter(IEnumerable<AbstractUICard> collection)
		{
			if (collectionsToFilter.Contains(collection))
			{
				return;
			}
			
			collectionsToFilter.Add(collection);
		}

		public void RemoveFromFilter(IEnumerable<AbstractUICard> collection)
		{
			collectionsToFilter.Remove(collection);
		}

		private void ApplyFilters(IEnumerable<AbstractUICard> cardCollection)
		{
			foreach (AbstractUICard abstractUICard in cardCollection)
			{
				abstractUICard.gameObject.SetActive(abstractUICard.MeetsFilters(currentFilters));
			}
		}

		private void FilterCollections()
		{
			collectionsToFilter.ForEach(ApplyFilters);
		}

		private void OnFiltersChanged(FiltersChangedEvent filtersChangedEvent)
		{
			currentFilters = filtersChangedEvent.filters;

			FilterCollections();
		}

		private void OnClickUICard()
		{
			FilterCollections();
		}
		
		private void OnToggleCurrentDeckFilter()
		{
			FilterCollections();
		}
		
		private void OnToggleAvailableCardsFilter()
		{
			FilterCollections();
		}
	}
}