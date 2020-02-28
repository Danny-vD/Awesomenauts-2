using System;
using Enums.Character;
using Interfaces;
using Interfaces.Audio;
using ScriptableObjects;
using ScriptableObjects.Audio.AudioSet;
using UnityEngine;

namespace Structs.Audio.AudioSetPerEnum
{
	[Serializable]
	public struct NautSetPerNaut : IAudioSetPerEnum<Awesomenaut, NautAudioSet>
	{
		public Awesomenaut Key
		{
			get => key;
			set => key = value;
		}

		public NautAudioSet Value
		{
			get => value;
			set => this.value = value;
		}

		[SerializeField]
		private Awesomenaut key;

		[SerializeField]
		private NautAudioSet value;

		public bool Equals(IKeyValuePair<Awesomenaut, NautAudioSet> other) => other != null && Key == other.Key;
	}
}