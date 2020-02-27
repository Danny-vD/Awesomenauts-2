using System.Collections.Generic;
using Enums.Audio;
using Interfaces.Audio;
using Structs.Audio;
using UnityEngine;
using Utility;

namespace ScriptableObjects
{
	[CreateAssetMenu(menuName = "Scriptable Objects/Audio/AnnouncerClips")]
	public class AnnouncerAudioSet : AAudioSet<AnnouncerSound, ClipDataPerAnnouncerSound>
	{
		[SerializeField]
		private List<ClipDataPerAnnouncerSound> clipDataPerSound;

		public override void PopulateDictionary()
		{
			FakeDictionaryUtil.PopulateEnumDictionary<ClipDataPerAnnouncerSound, AnnouncerSound, AudioClipData>(
				ref clipDataPerSound);
		}

		protected override List<ClipDataPerAnnouncerSound> GetClipDataPerSoundType() => clipDataPerSound;
	}
}