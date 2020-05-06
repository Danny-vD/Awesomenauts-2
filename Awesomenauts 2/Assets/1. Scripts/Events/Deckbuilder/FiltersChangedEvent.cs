using Enums.Character;
using Enums.Deckbuilder;
using VDFramework.EventSystem;

namespace Events.Deckbuilder
{
	public class FiltersChangedEvent : VDEvent
	{
		public readonly FilterValues Filters;
		public readonly Awesomenaut AwesomenautFilters;

		public FiltersChangedEvent(FilterValues filters, Awesomenaut awesomenautFilters)
		{
			this.Filters = filters;
			AwesomenautFilters = awesomenautFilters;
		}
	}
}