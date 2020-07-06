using VDFramework.SharedClasses.EventSystem;

namespace AwsomenautsCardGame.Events.Deckbuilder
{
	public class ToggleAvailableCardsFilterEvent : VDEvent
	{
		public readonly bool ShouldBeFiltered;

		public ToggleAvailableCardsFilterEvent(bool shouldBeFiltered)
		{
			ShouldBeFiltered = shouldBeFiltered;
		}
	}
}