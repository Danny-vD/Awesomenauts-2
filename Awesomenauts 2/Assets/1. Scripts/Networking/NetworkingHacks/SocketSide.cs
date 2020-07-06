using System;

namespace AwsomenautsCardGame.Networking.NetworkingHacks {
	[Flags]
	public enum SocketSide
	{
		SideA = 1,
		SideB = 2,
		RedSide = 4,
		BlueSide = 8,
		NonPlacable = 16,
		Neutral = 32,
	}
}