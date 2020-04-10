using Events.Deckbuilder;
using VDFramework.EventSystem;

namespace UI.Cards
{
	public class DeckUICard : AbstractUICard
	{
		protected override void OnPointerEnter()
		{
			EventManager.Instance.RaiseEvent(new HoverUICardEvent(this));
		}

		protected override void OnPointerClick()
		{
			EventManager.Instance.RaiseEvent(new ClickUICardEvent(this, true));
		}
	}
}