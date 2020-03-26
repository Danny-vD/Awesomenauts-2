using Commands;
using Events.CommandEvents;
using VDFramework.EventSystem;
using VDFramework.Singleton;

namespace Singleton
{
	public class CommandManager : Singleton<CommandManager>
	{
		private void OnEnable()
		{
			AddListeners();
		}

		private void OnDisable()
		{
			RemoveListeners();
		}

		private void AddListeners()
		{
			EventManager.Instance.AddListener<SendCommandEvent>(OnSendCommand);
		}

		private void RemoveListeners()
		{
			EventManager.Instance.RemoveListener<SendCommandEvent>(OnSendCommand);
		}

		private void OnSendCommand(SendCommandEvent sendCommandEvent)
		{
			SendToServer(sendCommandEvent.CommandToSend);
		}

		private void SendToServer(ACommand command)
		{
			// TODO: Tim, you handle the networking here right?
			// Do tell if you need more than this
		}
	}
}