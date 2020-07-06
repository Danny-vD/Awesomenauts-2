using AwsomenautsCardGame.Events.Deckbuilder;
using VDFramework.SharedClasses.EventSystem;

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