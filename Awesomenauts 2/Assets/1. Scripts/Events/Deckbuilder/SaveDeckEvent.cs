using System.Collections.Generic;
using System.Linq;
using VDFramework.SharedClasses.EventSystem;

namespace AwsomenautsCardGame.Events.Deckbuilder
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