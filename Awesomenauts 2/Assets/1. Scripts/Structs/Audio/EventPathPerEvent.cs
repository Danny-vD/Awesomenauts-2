using System;
using Interfaces;
using UnityEngine;
using EventType = Enums.Audio.EventType;

namespace Structs.Audio
{
	[Serializable]
	public struct EventPathPerEvent : IKeyValuePair<EventType, string>
	{
		[SerializeField]
		private EventType key;

		[SerializeField, FMODUnity.EventRef]
		private string value;

		public EventType Key
		{
			get => key;
			set => key = value;
		}

		public string Value
		{
			get => value;
			set => this.value = value;
		}

		public bool Equals(IKeyValuePair<EventType, string> other)
		{
			return other != null && other.Key == Key;
		}
	}
}