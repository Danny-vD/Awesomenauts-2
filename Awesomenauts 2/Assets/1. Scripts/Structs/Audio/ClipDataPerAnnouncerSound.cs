using System;
using Enums.Audio;
using Interfaces;
using Interfaces.Audio;
using UnityEngine;

namespace Structs.Audio
{
	[Serializable]
	public struct ClipDataPerAnnouncerSound : IAudioClipDataPerSoundType<AnnouncerSound>
	{
		public AnnouncerSound Key
		{
			get => SoundType;
			set => SoundType = value;
		}

		public AudioClipData Value
		{
			get => ClipData;
			set => ClipData = value;
		}
		
		public AnnouncerSound SoundType;
		public AudioClipData ClipData;

		public ClipDataPerAnnouncerSound(AnnouncerSound soundType = default)
		{
			SoundType = soundType;
			ClipData = default;
		}

		public ClipDataPerAnnouncerSound(AnnouncerSound soundType, params AudioClip[] clips)
		{
			SoundType = soundType;
			ClipData = default;

			foreach (AudioClip clip in clips)
			{
				ClipData.AudioClips.Add(clip);
			}
		}

		// Used by List<AnnouncerClipsPerSoundType>.Contains(AnnouncerClipsPerSoundType);
		public bool Equals(IKeyValuePair<AnnouncerSound, AudioClipData> other) => other != null && Key == other.Key;
	}
}