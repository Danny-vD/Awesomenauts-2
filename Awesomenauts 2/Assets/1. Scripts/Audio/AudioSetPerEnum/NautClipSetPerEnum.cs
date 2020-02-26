using System;
using Enums.Character;
using Interfaces;
using Interfaces.Audio;
using UnityEngine;

namespace Audio.AudioSetPerEnum
{
	[Serializable]
	public class NautClipSetPerNaut : IAudioSetPerEnum<Awesomenaut>
	{
		public Awesomenaut Key
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
		private Awesomenaut key = default;

		[SerializeField]
		private ScriptableObject value = default;

		public bool Equals(IKeyValuePair<Awesomenaut, ScriptableObject> other) => other != null && Key == other.Key;
	}
}