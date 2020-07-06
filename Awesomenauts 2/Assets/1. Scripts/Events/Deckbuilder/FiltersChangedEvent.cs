using AwsomenautsCardGame.Enums.Character;
using AwsomenautsCardGame.Enums.Deckbuilder;
using VDFramework.SharedClasses.EventSystem;

namespace AwsomenautsCardGame.Events.Deckbuilder
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