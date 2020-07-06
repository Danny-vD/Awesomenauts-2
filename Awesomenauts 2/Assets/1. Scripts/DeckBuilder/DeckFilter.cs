using System.Collections.Generic;
using VDFramework.SharedClasses.EventSystem;
using AwsomenautsCardGame.Enums.Character;
using AwsomenautsCardGame.Enums.Deckbuilder;
using AwsomenautsCardGame.Events.Deckbuilder;
using AwsomenautsCardGame.UI.Cards;

namespace AwsomenautsCardGame.DeckBuilder
{
	public class DeckFilter
	{
		private FilterValues currentFilters = FilterValues.ShowAll;
		private Awesomenaut currentAwesomenautFilters = Awesomenaut.All;

		private readonly List<IEnumerable<AbstractUICard>>
			collectionsToFilter = new List<IEnumerable<AbstractUICard>>();

		public DeckFilter()
		{
			AddListeners();
		}

		public void Destroy()
		{
			collectionsToFilter.Clear();
			RemoveListeners();
		}

		private void AddListeners()
		{
			EventManager.Instance.AddListener<FiltersChangedEvent>(OnFiltersChanged);
			EventManager.Instance.AddListener<ClickUICardEvent>(OnClickUICard, int.MinValue);
			EventManager.Instance.AddListener<ToggleCurrentDeckFilterEvent>(OnToggleCurrentDeckFilter, int.MinValue);
			EventManager.Instance.AddListener<ToggleAvailableCardsFilterEvent>(OnToggleAvailableCardsFilter,
				int.MinValue);
		}

		private void RemoveListeners()
		{
			if (!EventManager.IsInitialized)
			{
				return;
			}

			EventManager.Instance.RemoveListener<ClickUICardEvent>(OnClickUICard);
		}

		public static void SetFilters(FilterValues filters, Awesomenaut awesomenautFilters)
		{
			EventManager.Instance.RaiseEvent(new FiltersChangedEvent(filters, awesomenautFilters));
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
				abstractUICard.gameObject.SetActive(abstractUICard.MeetsFilters(currentFilters,
					currentAwesomenautFilters));
			}
		}

		private void FilterCollections()
		{
			collectionsToFilter.ForEach(ApplyFilters);
		}

		private void OnFiltersChanged(FiltersChangedEvent filtersChangedEvent)
		{
			currentFilters = filtersChangedEvent.Filters;
			currentAwesomenautFilters = filtersChangedEvent.AwesomenautFilters;

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