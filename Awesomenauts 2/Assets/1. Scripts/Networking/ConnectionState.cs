namespace AwsomenautsCardGame.Networking {
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