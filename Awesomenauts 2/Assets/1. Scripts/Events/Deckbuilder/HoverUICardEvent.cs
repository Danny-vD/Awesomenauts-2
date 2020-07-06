using AwsomenautsCardGame.UI.Cards;
using VDFramework.SharedClasses.EventSystem;

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