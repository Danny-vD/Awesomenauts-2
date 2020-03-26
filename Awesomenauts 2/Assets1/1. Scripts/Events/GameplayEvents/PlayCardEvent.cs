using VDFramework.EventSystem;

namespace Events.GameplayEvents
{
	public class PlayCardEvent : VDEvent
	{
		public readonly ICard CardToPlay;
		public readonly CardSocket CardSocket;

		public PlayCardEvent(ICard cardToPlay, CardSocket cardSocket)
		{
			CardToPlay = cardToPlay;
			CardSocket = cardSocket;
		}
	}
}