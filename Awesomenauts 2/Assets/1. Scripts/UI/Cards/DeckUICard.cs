using VDFramework.SharedClasses.EventSystem;
using AwsomenautsCardGame.Events.Deckbuilder;

namespace AwsomenautsCardGame.UI.Cards
{
	public class DeckUICard : AbstractUICard
	{
		protected override void OnPointerClick()
		{
			EventManager.Instance.RaiseEvent(new ClickUICardEvent(this, true));
		}
	}
}