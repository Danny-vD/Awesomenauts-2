using VDFramework.SharedClasses.EventSystem;
using AwsomenautsCardGame.UI.Cards;

namespace AwsomenautsCardGame.Events.Deckbuilder
{
	public class ClickUICardEvent : VDEvent
	{
		public readonly AbstractUICard Card;
		public readonly bool CardIsInDeck;

		public ClickUICardEvent(AbstractUICard card, bool isInDeck)
		{
			Card = card;
			CardIsInDeck = isInDeck;
		}
	}
}