using DeckBuilder;
using UnityEngine;
using VDFramework.EventSystem;

namespace Events.Deckbuilder
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