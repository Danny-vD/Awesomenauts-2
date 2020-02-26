using System;
using Enums.Announcer;
using Interfaces.Audio;
using UnityEngine;

namespace Audio.AudioSetPerEnum
{
	[Serializable]
	public class AnnouncerClipSetPerAnnouncer : IAudioSetPerEnum<Announcer>
	{
		public Announcer Key => key;

		public ScriptableObject Value => value;

		[SerializeField]
		private Announcer key = default;

		[SerializeField]
		private ScriptableObject value = default;

		public bool Equals(IAudioSetPerEnum<Announcer> other) => throw new System.NotImplementedException();
	}
}