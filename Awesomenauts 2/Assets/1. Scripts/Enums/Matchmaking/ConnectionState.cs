namespace AwsomenautsCardGame.Enums.Matchmaking {
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