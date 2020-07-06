using System.Collections.Generic;
using System.Linq;
using VDFramework.SharedClasses.EventSystem;
using AwsomenautsCardGame.Enums.Deckbuilder;

namespace AwsomenautsCardGame.Events.Deckbuilder
{
	public class SortingsChangedEvent : VDEvent
	{
		public readonly List<SortValue> Sortings;

		public SortingsChangedEvent(List<SortValue> sortings)
		{
			Sortings = sortings;
		}

		public SortingsChangedEvent(params SortValue[] sortings) : this(sortings.ToList()) { }
	}
}