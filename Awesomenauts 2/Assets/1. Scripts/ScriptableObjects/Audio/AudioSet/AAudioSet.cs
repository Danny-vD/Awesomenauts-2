﻿using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces.Audio;
using Structs.Audio;
using UnityEngine;
using Utility;

namespace ScriptableObjects.Audio.AudioSet
{
	public abstract class AAudioSet<TSoundType, TClipDataPerSoundType> : ScriptableObject, IAudioSet
		where TSoundType : struct, Enum
		where TClipDataPerSoundType : IAudioClipDataPerSoundType<TSoundType>, new()
	{
		public AudioClipData this[TSoundType soundType] => GetAudioClipData(soundType);

		/// <summary>
		/// Will fill the list with every value of the enum.
		/// <para>Use <see cref="FakeDictionaryUtil"/> class when overriding.</para> 
		/// </summary>
		public abstract void PopulateDictionary();
		
		public AudioClipData GetAudioClipData(TSoundType soundType)
		{
			foreach (TClipDataPerSoundType pair in GetClipDataPerSoundType().Where(pair => Equals(pair.Key, soundType)))
			{
				return pair.Value;
			}

			return AudioClipData.Null;
		}

		public bool ContainsKey(TSoundType soundType)
		{
			return (GetAudioClipData(soundType) != AudioClipData.Null);
		}

		protected abstract List<TClipDataPerSoundType> GetClipDataPerSoundType();
	}
}