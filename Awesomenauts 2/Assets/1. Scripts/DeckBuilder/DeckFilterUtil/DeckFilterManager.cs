using System.Linq;
using Enums.Character;
using Enums.Deckbuilder;
using UI.Cards;

namespace DeckBuilder.DeckFilterUtil
{
	public static class DeckFilterManager
	{
		private static FilterValues currentFilters = FilterValues.ShowAll;
		private static Awesomenaut currentAwesomenautFilters = Awesomenaut.All;
		
		public static void AddFilterFlagToCard(AbstractUICard card, FilterValues flagToAdd)
		{
			card.Filters |= flagToAdd;
		}

		public static void RemoveFilterFlagFromCard(AbstractUICard card, FilterValues flagToRemove)
		{
			card.Filters &= ~flagToRemove;
		}
		
		public static void AddIsInDeckFilter(AbstractUICard card)
		{
			AddFilterFlagToCard(card, FilterValues.IsIndeck);
			RemoveFilterFlagFromCard(card, FilterValues.IsNotInDeck);
		}

		public static void AddIsNotInDeckFilter(AbstractUICard card)
		{
			AddFilterFlagToCard(card, FilterValues.IsNotInDeck);
			RemoveFilterFlagFromCard(card, FilterValues.IsIndeck);
		}

		public static void SetFilters(params FilterValues[] filters)
		{
			FilterValues values = filters.Aggregate<FilterValues, FilterValues>(0, (current, filterValue) => current | filterValue);
			currentFilters = values;
			DeckFilter.SetFilters(values, currentAwesomenautFilters);
		}

		public static void AddFilters(params FilterValues[] filters)
		{
			FilterValues values = filters.Aggregate(currentFilters, (current, filterValue) => current | filterValue);
			currentFilters = values;
			DeckFilter.SetFilters(values, currentAwesomenautFilters);
		}
		
		public static void RemoveFilters(params FilterValues[] filters)
		{
			FilterValues values = filters.Aggregate(currentFilters, (current, filterValue) => current & ~filterValue);
			currentFilters = values;
			DeckFilter.SetFilters(values, currentAwesomenautFilters);
		}
		
		public static void SetFilters(params Awesomenaut[] awesomenautFilters)
		{
			Awesomenaut values = awesomenautFilters.Aggregate<Awesomenaut, Awesomenaut>(0, (current, filterValue) => current | filterValue);
			currentAwesomenautFilters = values;
			DeckFilter.SetFilters(currentFilters, values);
		}
		
		public static void AddFilters(params Awesomenaut[] filters)
		{
			Awesomenaut values = filters.Aggregate(currentAwesomenautFilters, (current, filterValue) => current | filterValue);
			currentAwesomenautFilters = values;
			DeckFilter.SetFilters(currentFilters, values);
		}
		
		public static void RemoveFilters(params Awesomenaut[] filters)
		{
			Awesomenaut values = filters.Aggregate(currentAwesomenautFilters, (current, filterValue) => current & ~filterValue);
			currentAwesomenautFilters = values;
			DeckFilter.SetFilters(currentFilters, values);
		}
	}
}