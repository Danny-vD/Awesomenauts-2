using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
		private bool playSound = false;

		[SerializeField]
		private string AnimationName;
		[SerializeField]
		private float AnimationLength;

		[SerializeField]
		private EventType soundToPlay = EventType.SFX_CARDS_CardPlace;

		public string Description;
		public virtual EffectTrigger Trigger => EffectTrigger.AfterPlay;


		public abstract IEnumerator TriggerEffect(Card c, CardSocket containingSocket, CardSocket targetSocket);

		public void _TriggerEffect(Card c, CardSocket containingSocket, CardSocket targetSocket)
		{
			c.StartCoroutine(InternalTrigger(c, containingSocket, targetSocket));
		}

		private IEnumerator InternalTrigger(Card c, CardSocket containingSocket, CardSocket targetSocket)
		{
			if (playSound)
			{
				PlaySound(soundToPlay, c.gameObject);
			}
			if (c.Animator != null && !string.IsNullOrEmpty(AnimationName))
			{
				c.Animator.Play(AnimationName);
				yield return new WaitForSeconds(AnimationLength);
			}



			yield return TriggerEffect(c, containingSocket, targetSocket);
		}

		private static void PlaySound(EventType @event, GameObject @object)
		{
			AudioPlayer.PlaySound(@event, @object);
		}
	}
}