using System;
using Enums.Announcer;
using Interfaces;
using Interfaces.Audio;
using ScriptableObjects;
using UnityEngine;

namespace Structs.Audio.AudioSetPerEnum
{
	[Serializable]
	public struct AnnouncerSetPerAnnouncer : IAudioSetPerEnum<Announcer, AnnouncerAudioSet>
	{
		public Announcer Key
		{
			get => key;
			set => key = value;
		}

		public AnnouncerAudioSet Value
		{
			get => value;
			set => this.value = value;
		}

		[SerializeField]
		private Announcer key;

		[SerializeField]
		private AnnouncerAudioSet value;

		public bool Equals(IKeyValuePair<Announcer, AnnouncerAudioSet> other) => other != null && Key == other.Key;
	}
}