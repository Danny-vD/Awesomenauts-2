using System;
using System.Collections.Generic;
using Audio.AudioSetPerEnum;
using Enums.Announcer;
using Enums.Audio;
using Enums.Character;
using Events.Audio;
using UnityEngine;
using VDFramework.EventSystem;
using VDFramework.Singleton;

namespace Audio
{
	public class AudioManager : Singleton<AudioManager>
	{
		[SerializeField]
		private List<NautClipSetPerNaut> nautClips;

		[SerializeField]
		private List<AnnouncerClipSetPerAnnouncer> announcerClips;

		private void OnEnable()
		{
			AddListeners();
		}

		private void OnDisable()
		{
			RemoveListeners();
		}

		private void AddListeners()
		{
			EventManager.Instance.AddListener<PlayAudioTypeEvent<NautSound, Awesomenaut>>(OnPlayAudioTypeEvent);
			EventManager.Instance.AddListener<PlayAudioTypeEvent<AnnouncerSound, Announcer>>(OnPlayAudioTypeEvent);
		}

		private static void OnPlayAudioTypeEvent<TAudioType, TAudioSet>(PlayAudioTypeEvent<TAudioType, TAudioSet> obj)
			where TAudioType : struct, Enum
			where TAudioSet : struct, Enum
		{
			print($"{obj.SoundTypeToPlay}: {obj.SetToUse}");
		}

		private void RemoveListeners()
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