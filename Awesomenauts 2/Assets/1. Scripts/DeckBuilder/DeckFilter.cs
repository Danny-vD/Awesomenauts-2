using System.Collections.Generic;
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
		}
		
		private void RemoveListeners()
		{
			if (!EventManager.IsInitialized)
			{
				return;
			}
    
			EventManager.Instance.RemoveListener<ClickUICardEvent>(OnClickUICard);
		}

		public void AddToFilter(IEnumerable<AbstractUICard> collection)
		{
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

		private void OnFiltersChanged(FiltersChangedEvent filtersChangedEvent)
		{
			currentFilters = filtersChangedEvent.filters;

			collectionsToFilter.ForEach(ApplyFilters);
		}

		private void OnClickUICard()
		{
			collectionsToFilter.ForEach(ApplyFilters);
		}
	}
}