using Events.Deckbuilder;
using UnityEngine;
using VDFramework.EventSystem;

namespace DeckBuilder
{
	public class AvailableUICard : AbstractUICard
	{
		protected override void OnPointerEnter()
		{
			EventManager.Instance.RaiseEvent(new HoverUICardEvent(this));
		}

		protected override void OnPointerClick()
		{
			EventManager.Instance.RaiseEvent(new ClickUICardEvent(this, false));
		}
	}
}