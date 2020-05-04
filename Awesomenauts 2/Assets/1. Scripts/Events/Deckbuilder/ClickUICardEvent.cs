using UI.Cards;
using VDFramework.EventSystem;

namespace Events.Deckbuilder
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