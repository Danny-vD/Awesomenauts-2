using System.Collections.Generic;
using Enums.Audio;
using Interfaces.Audio;
using Structs.Audio;
using UnityEngine;
using Utility;

namespace ScriptableObjects
{
	[CreateAssetMenu(menuName = "Scriptable Objects/Audio/NautClips")]
	public class NautAudioSet : AAudioSet<NautSound, ClipDataPerNautSound>
	{
		[SerializeField]
		private List<ClipDataPerNautSound> clipDataPerSound = new List<ClipDataPerNautSound>();

		private void Awake()
		{
			if (clipDataPerSound == null)
			{
				clipDataPerSound = new List<ClipDataPerNautSound>();
			}
		}

		protected override List<ClipDataPerNautSound> GetClipDataPerSoundType() => clipDataPerSound;

		public override void PopulateDictionary()
		{
			FakeDictionaryUtil.PopulateEnumDictionary<ClipDataPerNautSound, NautSound, AudioClipData>(
				ref clipDataPerSound);
		}
	}
}