using System;
using Enums.Character;
using Interfaces.Audio;
using UnityEngine;

namespace Audio.AudioSetPerEnum
{
	[Serializable]
	public class NautClipSetPerNaut : IAudioSetPerEnum<Awesomenaut>
	{
		public Awesomenaut Key => key;

		public ScriptableObject Value => value;

		[SerializeField]
		private Awesomenaut key = default;

		[SerializeField]
		private ScriptableObject value = default;

		public bool Equals(IAudioSetPerEnum<Awesomenaut> other) => other != null && other.Key == Key;
	}
}