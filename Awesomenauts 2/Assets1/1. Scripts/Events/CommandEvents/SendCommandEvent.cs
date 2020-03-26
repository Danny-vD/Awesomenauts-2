using Commands;
using VDFramework.EventSystem;

namespace Events.CommandEvents
{
	public class SendCommandEvent : VDEvent
	{
		public readonly ACommand CommandToSend;

		public SendCommandEvent(ACommand commandToSend)
		{
			CommandToSend = commandToSend;
		}
	}
}