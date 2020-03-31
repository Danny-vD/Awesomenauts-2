namespace Networking {
	public enum ConnectionState
	{
		Idle,
		Connecting,
		Connected,
		FoundMatch,
		Queued,
		ReconnectLoop,
	}
}