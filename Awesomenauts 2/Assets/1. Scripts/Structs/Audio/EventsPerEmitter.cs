using System;
using AwsomenautsCardGame.Enums.Audio;
using AwsomenautsCardGame.Interfaces;
using UnityEngine;
using EventType = AwsomenautsCardGame.Enums.Audio.EventType;

namespace AwsomenautsCardGame.Structs.Audio
{
	[Serializable]
	public struct EventsPerEmitter : IKeyValuePair<EmitterType, Enums.Audio.EventType>
	{
		[SerializeField]
		private EmitterType key;

		[SerializeField]
		private Enums.Audio.EventType value;

		public EmitterType Key
		{
			get => key;
			set => key = value;
		}

		public Enums.Audio.EventType Value
		{
			get => value;
			set => this.value = value;
		}

		public bool Equals(IKeyValuePair<EmitterType, Enums.Audio.EventType> other)
		{
			return other != null && other.Key == Key;
		}
	}
}