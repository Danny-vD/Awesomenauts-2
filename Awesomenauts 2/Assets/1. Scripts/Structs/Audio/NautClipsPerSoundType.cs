using System;
using System.Collections.Generic;
using Enums;
using Enums.Audio;
using Interfaces.Audio;
using UnityEngine;

namespace Structs.Audio
{
	[Serializable]
	public struct NautClipsPerSoundType : IEquatable<NautClipsPerSoundType>, IAudioClipsPerEnum<NautSoundType>
	{
		public NautSoundType Key
		{
			get => SoundType;
			set => SoundType = value;
		}

		public List<AudioClip> Value
		{
			get => Clips;
			set => Clips = value;
		}

		public bool ShouldLoop => IsLooping;

		public NautSoundType SoundType;
		public List<AudioClip> Clips;

		public bool IsLooping;

		public NautClipsPerSoundType(NautSoundType soundType = default)
		{
			SoundType = soundType;
			Clips = null;
			IsLooping = false;
		}
		
		public NautClipsPerSoundType(NautSoundType soundType, params AudioClip[] clips)
		{
			SoundType = soundType;
			Clips = new List<AudioClip>();

			IsLooping = false;

			foreach (AudioClip clip in clips)
			{
				Clips.Add(clip);
			}
		}

		// Used by List<AudioClipsPerSoundType>.Contains(AudioClipsPerSoundType);
		bool IEquatable<NautClipsPerSoundType>.Equals(NautClipsPerSoundType other)
		{
			return Equals(SoundType, other.SoundType);
		}
	}
}