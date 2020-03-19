using Events.GameplayEvents;
using VDFramework.EventSystem;

namespace Commands
{
	public class PlayCardCommand : ACommand
	{
		public ICard CardToPlay { get; set; }
		public CardSocket CardSocket { get; set; }


		public override void Execute()
		{
			EventManager.Instance.RaiseEvent(new PlayCardEvent(CardToPlay, CardSocket));
		}
	}
}