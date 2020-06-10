﻿using System.Collections.Generic;
using Enums.Audio;
using Structs.Audio;
using UnityEngine;
using VDFramework.Singleton;

namespace Audio
{
	public class AudioManager : Singleton<AudioManager>
	{
		public EventPaths EventPaths;
		
		public List<InitialValuePerBus> initialVolumes = new List<InitialValuePerBus>();
		
		protected override void Awake()
		{
			base.Awake();
			EventPaths.AddEmitters(gameObject);

			DontDestroyOnLoad(gameObject);
			
			SetInitialVolumes();
			AudioPlayer.PlayEmitter(EmitterType.BackgroundMusic);

			//print(GetVolume(BusType.Music));
		}

		private void SetInitialVolumes()
		{
			foreach (InitialValuePerBus pair in initialVolumes)
			{
				if (pair.Key == BusType.Master)
				{
					AudioParameterManager.SetMasterVolume(pair.Value);
					AudioParameterManager.SetMasterMute(pair.isMuted);
					continue;
				}
				
				string busPath = EventPaths.GetPath(pair.Key);
				AudioParameterManager.SetBusVolume(busPath, pair.Value);
				AudioParameterManager.SetBusMute(busPath, pair.isMuted);
			}
		}

		public void SetVolume(BusType busType, float volume)
		{
			AudioParameterManager.SetBusVolume(EventPaths.GetPath(busType), volume);
		}

		public float GetVolume(BusType busType)
		{
			return AudioParameterManager.GetBusVolume(EventPaths.GetPath(busType));
		}
	}
}