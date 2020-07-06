using AwsomenautsCardGame.Enums.Audio;
using FMODUnity;
using UnityEngine;
using EventType = AwsomenautsCardGame.Enums.Audio.EventType;

namespace AwsomenautsCardGame.Audio
{
	public static class AudioPlayer
	{
		public static void PlayEmitter(EmitterType emitter)
		{
			AudioManager.Instance.EventPaths.GetEmitter(emitter).Play();
		}

		public static void ToggleEmitter(EmitterType emitterType)
		{
			StudioEventEmitter emitter = AudioManager.Instance.EventPaths.GetEmitter(emitterType);

			if (emitter.IsPlaying())
			{
				emitter.Stop();
				return;
			}
			
			emitter.Play();
		}
		
		public static void StopEmitter(EmitterType emitter)
		{
			AudioManager.Instance.EventPaths.GetEmitter(emitter).Stop();
		}

		public static void PlaySound(Enums.Audio.EventType @event, GameObject location)
		{
			RuntimeManager.PlayOneShotAttached(AudioManager.Instance.EventPaths.GetPath(@event), location);
		}

		public static void PlaySound(Enums.Audio.EventType @event)
		{
			RuntimeManager.PlayOneShotAttached(AudioManager.Instance.EventPaths.GetPath(@event), AudioManager.Instance.gameObject);
		}
	}
}