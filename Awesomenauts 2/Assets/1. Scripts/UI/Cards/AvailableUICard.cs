using AwsomenautsCardGame.Events.Deckbuilder;
using VDFramework.SharedClasses.EventSystem;

namespace AwsomenautsCardGame.UI.Cards
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