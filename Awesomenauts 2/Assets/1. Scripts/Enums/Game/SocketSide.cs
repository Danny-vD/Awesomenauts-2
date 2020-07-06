using System;

namespace AwsomenautsCardGame.Enums.Game {
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