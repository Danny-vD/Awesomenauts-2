using System.Collections.Generic;
using Enums.Audio;
using Structs.Audio;
using UnityEngine;
using Utility;

namespace ScriptableObjects
{
	[CreateAssetMenu(menuName = "Scriptable Objects/Audio/AnnouncerClips")]
	public class AnnouncerAudioClips : AAudioClips<AnnouncerSound, AnnouncerClipsPerSoundType>
	{
		[SerializeField]
		private List<AnnouncerClipsPerSoundType> clipsPerEnum;
		
		public override void PopulateDictionary()
		{
			AudioClipsUtil.PopulateDictionary<AnnouncerSound, AnnouncerClipsPerSoundType>(ref clipsPerEnum);
		}

		protected override List<AnnouncerClipsPerSoundType> GetAudioClipsPerSoundType() => clipsPerEnum;
	}
}