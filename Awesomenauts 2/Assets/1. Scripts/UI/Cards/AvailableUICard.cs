using Events.Deckbuilder;
using VDFramework.EventSystem;

namespace UI.Cards
{
	public class AvailableUICard : AbstractUICard
	{
		protected override void OnPointerClick()
		{
			if (Amount > 0)
			{
				EventManager.Instance.RaiseEvent(new ClickUICardEvent(this, false));
			}
		}
	}
}