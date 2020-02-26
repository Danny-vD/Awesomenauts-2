using System.Collections.Generic;
using Enums.Audio;
using Structs.Audio;
using UnityEngine;
using Utility;

namespace ScriptableObjects
{
	[CreateAssetMenu(menuName = "Scriptable Objects/Audio/NautClips")]
	public class NautAudioClips : AAudioClips<NautSound, NautClipsPerSoundType>
	{
		[SerializeField]
		private List<NautClipsPerSoundType> clipsPerEnum = new List<NautClipsPerSoundType>();

		private void Awake()
		{
			if (clipsPerEnum == null)
			{
				clipsPerEnum = new List<NautClipsPerSoundType>();
			}
		}

		protected override List<NautClipsPerSoundType> GetAudioClipsPerSoundType() => clipsPerEnum;

		public override void PopulateDictionary()
		{
			FakeDictionaryUtil.PopulateEnumDictionary<NautClipsPerSoundType, NautSound, List<AudioClip>>(
				ref clipsPerEnum);
		}
	}
}