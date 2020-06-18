using Maps;
using Player;
using UnityEngine;

namespace Assets._1._Scripts.ScriptableObjects.Effects
{
	public abstract class AEffect : ScriptableObject
	{
		public string Description;
		public virtual EffectTrigger Trigger => EffectTrigger.AfterPlay;
		public abstract void TriggerEffect(Card c, CardSocket containingSocket, CardSocket targetSocket);
	}
}