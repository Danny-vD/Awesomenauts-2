using System;
using System.Collections.Generic;
using UnityEngine;

namespace Structs.Audio
{
	[Serializable]
	public struct AudioClipData : IEquatable<AudioClipData>
	{
		public List<AudioClip> AudioClips;

		public bool ShouldLoop;

		public static AudioClipData Null => new AudioClipData() {AudioClips = null};
		
		public AudioClipData(AudioClipData data = default)
		{
			AudioClips = new List<AudioClip>();

			ShouldLoop = false;
		}

		public AudioClipData(params AudioClip[] clips)
		{
			AudioClips = new List<AudioClip>();

			foreach (AudioClip audioClip in clips)
			{
				AudioClips.Add(audioClip);
			}

			ShouldLoop = false;
		}

		public bool Equals(AudioClipData other) => other.AudioClips == AudioClips;
		public override bool Equals(object obj) => obj is AudioClipData other && Equals(other);

		public static bool operator ==(AudioClipData lhs, AudioClipData rhs) => lhs.Equals(rhs);
		public static bool operator !=(AudioClipData lhs, AudioClipData rhs) => !(lhs == rhs);

		public override int GetHashCode() => base.GetHashCode();
	}
}