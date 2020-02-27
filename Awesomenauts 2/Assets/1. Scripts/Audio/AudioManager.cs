using System;
using System.Collections.Generic;
using Enums.Announcer;
using Enums.Audio;
using Enums.Character;
using Events.Audio;
using ScriptableObjects;
using Structs.Audio.AudioSetPerEnum;
using UnityEngine;
using Utility;
using VDFramework.EventSystem;
using VDFramework.Singleton;

namespace Audio
{
	public class AudioManager : Singleton<AudioManager>
	{
		[SerializeField]
		private List<NautSetPerNaut> nautClips;

		[SerializeField]
		private List<AnnouncerSetPerAnnouncer> announcerClips;

		private void OnEnable()
		{
			AddListeners();
		}

		private void OnDisable()
		{
			RemoveListeners();
		}

		public void UpdateDictioneries()
		{
			FakeDictionaryUtil.PopulateEnumDictionary<NautSetPerNaut, Awesomenaut, NautAudioSet>(ref nautClips);
			FakeDictionaryUtil.PopulateEnumDictionary<AnnouncerSetPerAnnouncer, Announcer, AnnouncerAudioSet>(
				ref announcerClips);
		}

		private static void OnPlayAudioTypeEvent<TAudioType, TAudioSet>(PlayAudioTypeEvent<TAudioType, TAudioSet> obj)
			where TAudioType : struct, Enum
			where TAudioSet : struct, Enum
		{
			print($"{obj.SoundTypeToPlay}: {obj.SetToUse}");
		}

		private static void AddListeners()
		{
			EventManager.Instance.AddListener<PlayAudioTypeEvent<NautSound, Awesomenaut>>(OnPlayAudioTypeEvent);
			EventManager.Instance.AddListener<PlayAudioTypeEvent<AnnouncerSound, Announcer>>(OnPlayAudioTypeEvent);
		}
		
		private static void RemoveListeners()
		{
			if (!EventManager.IsInitialized)
			{
				return;
			}

			EventManager.Instance.RemoveListener<PlayAudioTypeEvent<NautSound, Awesomenaut>>(OnPlayAudioTypeEvent);
			EventManager.Instance.RemoveListener<PlayAudioTypeEvent<AnnouncerSound, Announcer>>(
				OnPlayAudioTypeEvent);
		}
	}
}