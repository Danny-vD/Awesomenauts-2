using Maps;
using UnityEngine;

namespace Assets._1._Scripts.ScriptableObjects.Effects
{
	public abstract class AEffect : ScriptableObject
	{
		public virtual EffectTrigger Trigger => EffectTrigger.AfterPlay;
		public abstract void TriggerEffect(CardSocket containingSocket, CardSocket targetSocket);
	}
}