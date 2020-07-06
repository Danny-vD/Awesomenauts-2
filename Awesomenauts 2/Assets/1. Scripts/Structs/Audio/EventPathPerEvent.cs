using System;
using AwsomenautsCardGame.Interfaces;
using UnityEngine;
using EventType = AwsomenautsCardGame.Enums.Audio.EventType;

namespace AwsomenautsCardGame.Structs.Audio
{
	[Serializable]
	public struct EventPathPerEvent : IKeyValuePair<Enums.Audio.EventType, string>
	{
		[SerializeField]
		private Enums.Audio.EventType key;

		[SerializeField, FMODUnity.EventRef]
		private string value;

		public Enums.Audio.EventType Key
		{
			get => key;
			set => key = value;
		}

		public string Value
		{
			get => value;
			set => this.value = value;
		}

		public bool Equals(IKeyValuePair<Enums.Audio.EventType, string> other)
		{
			return other != null && other.Key == Key;
		}
	}
}