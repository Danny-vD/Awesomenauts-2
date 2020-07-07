using System.Collections.Generic;
using AwsomenautsCardGame.Enums.Audio;
using AwsomenautsCardGame.Enums.Cards;
using AwsomenautsCardGame.Structs.Audio;
using AwsomenautsCardGame.Utility;
using VDFramework.VDUnityFramework.Singleton;
using EventType = AwsomenautsCardGame.Enums.Audio.EventType;

namespace AwsomenautsCardGame.Audio
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
		}

		private void Start()
		{
			if (GameInitializer.Instance.GameData.Mute)
				AudioParameterManager.SetMasterMute(true);
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

		private static EventType GetAttackEventForCardType(CardType cardType)
		{
			return cardType == CardType.Ranged ? EventType.SFX_CARDS_RangedAttack : EventType.SFX_CARDS_MeleeAttack;
		}
	}
}