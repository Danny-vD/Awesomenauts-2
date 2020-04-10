using System.Collections.Generic;
using System.Linq;
using VDFramework.EventSystem;

namespace Events.Deckbuilder
{
	public class SaveDeckEvent : VDEvent
	{
		public readonly int[] CardIDs;

		public SaveDeckEvent(IEnumerable<int> cardIDs)
		{
			CardIDs = cardIDs.ToArray();
		}
	}
}