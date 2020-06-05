using System;
using Enums.Audio;
using FMODUnity;
using UnityEngine;
using VDFramework.Singleton;

namespace Audio
{
	public class AudioManager : Singleton<AudioManager>
	{
		public EventPaths eventPaths;
		
		protected override void Awake()
		{
			base.Awake();
			eventPaths.AddEmitters(gameObject);

			DontDestroyOnLoad(gameObject);
			
			AudioPlayer.PlayEmitter(EmitterType.BackgroundMusic);
		}
	}
}