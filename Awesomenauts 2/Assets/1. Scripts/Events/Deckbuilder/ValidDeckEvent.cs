using VDFramework.SharedClasses.EventSystem;

namespace AwsomenautsCardGame.Events.Deckbuilder
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