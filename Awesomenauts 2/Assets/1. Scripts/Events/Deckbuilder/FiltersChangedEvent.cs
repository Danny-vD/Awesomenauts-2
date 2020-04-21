using Enums.Deckbuilder;
using VDFramework.EventSystem;

namespace Events.Deckbuilder
{
	public class FiltersChangedEvent : VDEvent
	{
		public readonly FilterValues Filters;

		public FiltersChangedEvent(FilterValues filters)
		{
			this.Filters = filters;
		}
	}
}