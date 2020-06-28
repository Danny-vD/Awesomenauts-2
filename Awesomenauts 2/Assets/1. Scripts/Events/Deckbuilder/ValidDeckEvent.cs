using VDFramework.EventSystem;

namespace Events.Deckbuilder
{
	public class ValidDeckEvent : VDEvent
	{
		public readonly bool IsValid;

		public ValidDeckEvent(bool isValid)
		{
			IsValid = isValid;
		}
	}
}