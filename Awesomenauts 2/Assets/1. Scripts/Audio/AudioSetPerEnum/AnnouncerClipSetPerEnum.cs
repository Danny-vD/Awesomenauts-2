using System;
using Enums.Announcer;
using Interfaces;
using Interfaces.Audio;
using UnityEngine;

namespace Audio.AudioSetPerEnum
{
	[Serializable]
	public class AnnouncerClipSetPerAnnouncer : IAudioSetPerEnum<Announcer>
	{
		public Announcer Key
		{
			get => key;
			set => key = value;
		}

		public ScriptableObject Value
		{
			get => value;
			set => this.value = value;
		}

		[SerializeField]
		private Announcer key = default;

		[SerializeField]
		private ScriptableObject value = default;

		public bool Equals(IKeyValuePair<Announcer, ScriptableObject> other) => other != null && Key == other.Key;
	}
}