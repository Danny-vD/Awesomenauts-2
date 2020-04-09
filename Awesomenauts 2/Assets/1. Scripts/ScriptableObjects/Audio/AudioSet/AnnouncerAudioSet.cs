using System.Collections.Generic;
using Enums.Audio;
using Structs.Audio;
using Utility;
using UnityEngine;

namespace ScriptableObjects.Audio.AudioSet
{
	[CreateAssetMenu(menuName = "Scriptable Objects/Audio/AnnouncerClips")]
	public class AnnouncerAudioSet : AAudioSet<AnnouncerSound, ClipDataPerAnnouncerSound>
	{
		[SerializeField]
		private List<ClipDataPerAnnouncerSound> clipDataPerSound = new List<ClipDataPerAnnouncerSound>();

		public override void PopulateDictionary()
		{
			FakeDictionaryUtil.PopulateEnumDictionary<ClipDataPerAnnouncerSound, AnnouncerSound, AudioClipData>(
				clipDataPerSound);
		}

		protected override List<ClipDataPerAnnouncerSound> GetClipDataPerSoundType() => clipDataPerSound;
	}
}