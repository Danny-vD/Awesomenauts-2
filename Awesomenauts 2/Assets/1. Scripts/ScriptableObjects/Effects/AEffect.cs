using Audio;
using Maps;
using Player;
using UnityEngine;
using EventType = Enums.Audio.EventType;

namespace Assets._1._Scripts.ScriptableObjects.Effects
{
	public abstract class AEffect : ScriptableObject
	{
		[SerializeField]
		private bool playSound = true;

		[SerializeField]
		private EventType soundToPlay = EventType.SFX_CARDS_CardPlace;

		public string Description;
		public virtual EffectTrigger Trigger => EffectTrigger.AfterPlay;

		public virtual void TriggerEffect(Card c, CardSocket containingSocket, CardSocket targetSocket)
		{
			if (playSound)
			{
				PlaySound(soundToPlay, c.gameObject);
			}
		}

		private static void PlaySound(EventType @event, GameObject @object)
		{
			AudioPlayer.PlaySound(@event, @object);
		}
	}
}