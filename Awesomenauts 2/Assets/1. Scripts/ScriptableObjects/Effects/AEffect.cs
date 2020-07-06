using System.Collections;
using Assets._1._Scripts.AnimationSystem;
using Audio;
using Maps;
using Player;
using UnityEngine;
using EventType = Enums.Audio.EventType;

namespace Assets._1._Scripts.ScriptableObjects.Effects
{
	public abstract class AEffect : ScriptableObject
	{
		public bool playSound = false;

		public CardAnimation Animation;
		
		public EventType soundToPlay = EventType.SFX_CARDS_CardPlace;

		public string Description;
		public virtual EffectTrigger Trigger => EffectTrigger.AfterPlay;


		public abstract IEnumerator TriggerEffect(Card c, CardSocket containingSocket, CardSocket targetSocket);

		public void InvokeEffect(Card c, CardSocket containingSocket, CardSocket targetSocket)
		{
			c.StartCoroutine(InternalTrigger(c, containingSocket, targetSocket));
		}

		private IEnumerator InternalTrigger(Card c, CardSocket containingSocket, CardSocket targetSocket)
		{
			while (c.IsLocked)
			{
				Debug.Log("Source Card Locked..");
				yield return 1; //Just spin until the card is unlocked again
			}

			if (targetSocket != null && targetSocket.HasCard)
			{
				while (targetSocket.DockedCard.IsLocked)
				{
					Debug.Log("Target Card Locked..");
					yield return 1; //Just spin until the card is unlocked again
				}
				targetSocket.DockedCard.Lock(true);
			}

			c.Lock(true);

			if (playSound)
			{
				PlaySound(soundToPlay, c.gameObject);
			}
			if (c.Animator != null && Animation != null)
			{
				yield return c.Animator.Play(Animation, targetSocket?.CardTarget);
				//yield return new WaitForSeconds(AnimationLength);
			}

			yield return TriggerEffect(c, containingSocket, targetSocket);
			c.Lock(false);
			targetSocket?.DockedCard?.Lock(false);
		}

		private static void PlaySound(EventType @event, GameObject @object)
		{
			AudioPlayer.PlaySound(@event, @object);
		}

		public override string ToString()
		{
			return $"\t{GetType().Name}\n\tTrigger: {Trigger}\n\tDescription: {Description}";
		}
	}
}