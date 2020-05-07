using VDFramework.EventSystem;

namespace Events.Deckbuilder
{
	public class ToggleCurrentDeckFilterEvent : VDEvent
	{
		public readonly bool ShouldBeFiltered;

		public ToggleCurrentDeckFilterEvent(bool shouldBeFiltered)
		{
			ShouldBeFiltered = shouldBeFiltered;
		}
	}
}