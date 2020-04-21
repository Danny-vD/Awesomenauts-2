using System;

namespace Enums.Deckbuilder
{
	public enum SortValue
	{
		//Value doubles as priority, where low == high priority
		IsInDeck,
		Type,
		Name,
		Amount,
	}
}