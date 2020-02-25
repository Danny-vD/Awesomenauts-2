using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Interfaces.Audio;
using Structs.Audio;
using UnityEngine;
using Utility;

namespace ScriptableObjects
{
	[CreateAssetMenu(menuName = "Scriptable Objects/Audio/NautClips")]
	public class NautAudioClips : AAudioClips<NautSoundType>
	{
		[SerializeField]
		private List<NautClipsPerSoundType> clipsPerSoundType = new List<NautClipsPerSoundType>();

		protected override List<IAudioClipsPerEnum<NautSoundType>> GetAudioClipsPerSoundType() =>
			clipsPerSoundType.Select(@struct => (IAudioClipsPerEnum<NautSoundType>) @struct).ToList();

		public void PopulateDictionary()
		{
			AudioClipsUtil.PopulateDictionary<NautSoundType, NautClipsPerSoundType>(ref clipsPerSoundType);
		}
	}
}