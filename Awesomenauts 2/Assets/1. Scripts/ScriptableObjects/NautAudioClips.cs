using System.Collections.Generic;
using System.Linq;
using Enums.Audio;
using Structs.Audio;
using UnityEngine;
using Utility;
using VDFramework.Extensions;

namespace ScriptableObjects
{
	[CreateAssetMenu(menuName = "Scriptable Objects/Audio/NautClips")]
	public class NautAudioClips : AAudioClips<NautSoundType, NautClipsPerSoundType>
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

		[ContextMenu("Refresh lists")]
		private void Refresh()
		{
			PopulateDictionary();
		}
		
		protected override List<NautClipsPerSoundType> GetAudioClipsPerSoundType() => clipsPerEnum;

		public override void PopulateDictionary()
		{
			AudioClipsUtil.PopulateDictionary<NautSoundType, NautClipsPerSoundType>(ref clipsPerEnum);
		}
	}
}