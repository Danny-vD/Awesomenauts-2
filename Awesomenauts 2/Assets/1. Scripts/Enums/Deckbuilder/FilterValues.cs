using System;

namespace Enums.Deckbuilder
{
	[Flags]
	public enum FilterValues
	{
		ShowAll = -1,
		Owned = 1,
		Action = 2,
		Melee = 4,
		Ranged = 8,
		Tank = 16,
	}
}