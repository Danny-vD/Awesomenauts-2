using System;
using Enums.Audio;
using Interfaces;
using Interfaces.Audio;
using UnityEngine;

namespace Structs.Audio
{
	[Serializable]
	public struct ClipDataPerNautSound : IAudioClipDataPerSoundType<NautSound>
	{
		public NautSound Key
		{
			get => SoundType;
			set => SoundType = value;
		}

		public AudioClipData Value
		{
			get => ClipData;
			set => ClipData = value;
		}
		
		public NautSound SoundType;
		public AudioClipData ClipData;

		public ClipDataPerNautSound(NautSound soundType = default)
		{
			SoundType = soundType;
			ClipData = default;
		}
		
		public ClipDataPerNautSound(NautSound soundType, params AudioClip[] clips)
		{
			SoundType = soundType;
			ClipData = default;

			foreach (AudioClip clip in clips)
			{	
				ClipData.AudioClips.Add(clip);
			}
		}

		// Used by List<NautClipsPerSoundType>.Contains(NautClipsPerSoundType);
		public bool Equals(IKeyValuePair<NautSound, AudioClipData> other) => other != null && Key == other.Key;
	}
}