﻿using Enums.Deckbuilder;
using VDFramework.EventSystem;

namespace Events.Deckbuilder
{
	public class FiltersChangedEvent : VDEvent
	{
		public readonly FilterValues filters;

		public FiltersChangedEvent(FilterValues filters)
		{
			this.filters = filters;
		}
	}
}