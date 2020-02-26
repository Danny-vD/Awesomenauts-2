using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces.Audio;
using UnityEngine;
using Utility;
using VDFramework.Extensions;

namespace ScriptableObjects
{
	public abstract class AAudioClips<TEnum, TStruct> : ScriptableObject
		where TEnum : struct, Enum
		where TStruct : IAudioClipsPerEnum<TEnum>, new()
	{
		private List<TStruct> clipsPerSoundType;

		public AudioClip this[TEnum soundType] => GetClip(soundType, out _);
		public AudioClip this[TEnum soundType, int index] => GetClip(soundType, out _, index);

		private void Awake()
		{
			clipsPerSoundType = GetAudioClipsPerSoundType();
		}
		
		/// <summary>
		/// Will fill the list with every value of the enum.
		/// <para>Use <see cref="AudioClipsUtil"/> class when overriding.</para> 
		/// </summary>
		public abstract void PopulateDictionary();

		public AudioClip GetClip(TEnum soundType, out bool shouldLoop, int index = -1)
		{
			List<AudioClip> audioClips = GetAudioClips(soundType, out shouldLoop);

			if (audioClips == null)
			{
				return null;
			}

			if (index == -1)
			{
				return audioClips.GetRandomItem();
			}

			return audioClips[index];
		}

		public AudioClip GetRandomClip(TEnum soundType, out bool shouldLoop, out int index)
		{
			List<AudioClip> audioClips = GetAudioClips(soundType, out shouldLoop);

			if (audioClips == null)
			{
				index = -1;
				return null;
			}

			return audioClips.GetRandomItem(out index);
		}

		public bool ContainsKey(TEnum soundType)
		{
			return (GetAudioClips(soundType) != null);
		}

		public int GetClipCount(TEnum soundType)
		{
			return GetAudioClips(soundType).Count;
		}

		protected abstract List<TStruct> GetAudioClipsPerSoundType();

		private List<AudioClip> GetAudioClips(TEnum soundType, out bool shouldLoop)
		{
			foreach (TStruct pair in clipsPerSoundType.Where(pair => Equals(pair.Key, soundType)))
			{
				shouldLoop = pair.ShouldLoop;

				return pair.Value;
			}

			shouldLoop = false;
			return null;
		}

		private List<AudioClip> GetAudioClips(TEnum soundType)
		{
			return GetAudioClips(soundType, out _);
		}
	}
}