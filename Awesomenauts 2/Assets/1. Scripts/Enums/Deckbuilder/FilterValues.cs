using System;

namespace AwsomenautsCardGame.Enums.Deckbuilder
{
	[Flags]
	public enum FilterValues
	{
		ShowAll = -1,
		Owned = 1,
		IsInDeck = 2,
		IsNotInDeck = 4,
		Action = 8,
		Melee = 16,
		Ranged = 32,
		Tank = 64,
		NoTargetAction = 128,
	}
}