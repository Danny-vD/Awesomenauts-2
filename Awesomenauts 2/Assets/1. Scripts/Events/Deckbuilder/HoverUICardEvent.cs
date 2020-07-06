using VDFramework.SharedClasses.EventSystem;
using AwsomenautsCardGame.UI.Cards;

namespace AwsomenautsCardGame.Events.Deckbuilder
{
	public class HoverUICardEvent : VDEvent
	{
		public readonly AbstractUICard Card;

		public HoverUICardEvent(AbstractUICard card)
		{
			Card = card;
		}
	}
}