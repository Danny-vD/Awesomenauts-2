using System;
using System.Collections.Generic;
using Enums.Audio;
using Interfaces.Audio;
using UnityEngine;

namespace Structs.Audio
{
	[Serializable]
	public struct AnnouncerClipsPerSoundType : IEquatable<AnnouncerClipsPerSoundType>, IAudioClipsPerEnum<AnnouncerSoundType>
	{
		public AnnouncerSoundType Key
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

		public AnnouncerSoundType SoundType;
		public List<AudioClip> Clips;

		public bool IsLooping;

		public AnnouncerClipsPerSoundType(AnnouncerSoundType soundType = default)
		{
			SoundType = soundType;
			Clips = null;
			IsLooping = false;
		}
		
		public AnnouncerClipsPerSoundType(AnnouncerSoundType soundType, params AudioClip[] clips)
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
		bool IEquatable<AnnouncerClipsPerSoundType>.Equals(AnnouncerClipsPerSoundType other)
		{
			return Equals(SoundType, other.SoundType);
		}
	}
}