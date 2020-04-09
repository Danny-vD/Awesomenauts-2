using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Enums.Announcer;
using Enums.Audio;
using Enums.Character;
using Events.Audio;
using ScriptableObjects.Audio.AudioSet;
using Structs.Audio;
using Structs.Audio.AudioSetPerEnum;
using Utility;
using VDFramework.SharedClasses.EventSystem;
using VDFramework.VDUnityFramework.Singleton;
using UnityEngine;

namespace Audio
{
	public class AudioManager : Singleton<AudioManager>
	{
		[SerializeField]
		private List<NautSetPerNaut> nautClips = new List<NautSetPerNaut>();

		[SerializeField]
		private List<AnnouncerSetPerAnnouncer> announcerClips = new List<AnnouncerSetPerAnnouncer>();

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
			FakeDictionaryUtil.PopulateEnumDictionary<NautSetPerNaut, Awesomenaut, NautAudioSet>(nautClips);
			FakeDictionaryUtil.PopulateEnumDictionary<AnnouncerSetPerAnnouncer, Announcer, AnnouncerAudioSet>(
				announcerClips);
		}

		private void OnPlayAudioTypeEvent<TEnum, TSoundToPlay>(PlayAudioTypeEvent<TEnum, TSoundToPlay> @event)
			where TEnum : struct, Enum
			where TSoundToPlay : struct, Enum
		{
			EventManager.Instance.RaiseEvent(new PlayAudioClipEvent(GetCorrectClipData(@event)));
		}

		private AudioClipData GetCorrectClipData<TEnum, TSoundToPlay>(PlayAudioTypeEvent<TEnum, TSoundToPlay> @event)
			where TEnum : struct, Enum
			where TSoundToPlay : struct, Enum
		{
			//TODO: make proper, this is garbage

			if (@event.EnumName is Awesomenaut awesomenaut)
			{
				NautSetPerNaut setPerEnum = nautClips.First(set => set.Key == awesomenaut);

				if (@event.SoundTypeToPlay is NautSound sound)
				{
					return setPerEnum.Value.GetAudioClipData(sound);
				}

				throw new InvalidEnumArgumentException($"{typeof(TSoundToPlay)} is not a NautSound");
			}

			if (@event.EnumName is Announcer announcer)
			{
				AnnouncerSetPerAnnouncer setPerEnum = announcerClips.First(set => set.Key == announcer);

				if (@event.SoundTypeToPlay is AnnouncerSound sound)
				{
					return setPerEnum.Value.GetAudioClipData(sound);
				}

				throw new InvalidEnumArgumentException($"{typeof(TSoundToPlay)} is not an AnnouncerSound");
			}

			throw new InvalidEnumArgumentException($"{typeof(TEnum)} is not valid");
		}

		private void AddListeners()
		{
			EventManager.Instance.AddListener<PlayAudioTypeEvent<Awesomenaut, NautSound>>(OnPlayAudioTypeEvent);
			EventManager.Instance.AddListener<PlayAudioTypeEvent<Announcer, AnnouncerSound>>(OnPlayAudioTypeEvent);
		}

		private void RemoveListeners()
		{
			if (!EventManager.IsInitialized)
			{
				return;
			}

			EventManager.Instance.RemoveListener<PlayAudioTypeEvent<Awesomenaut, NautSound>>(OnPlayAudioTypeEvent);
			EventManager.Instance.RemoveListener<PlayAudioTypeEvent<Announcer, AnnouncerSound>>(
				OnPlayAudioTypeEvent);
		}
	}
}