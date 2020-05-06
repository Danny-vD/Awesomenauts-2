using System;
using System.Collections.Generic;
using Assets._1._Scripts.ScriptableObjects.Effects;
using Maps;

namespace Player {
	[Serializable]
	public class EffectManager
	{
		public List<AEffect> Effects;

		public void TriggerEffects(EffectTrigger trigger, CardSocket containingSocket, CardSocket targetCardSocket)
		{
			foreach (AEffect aEffect in Effects)
			{
				if((aEffect.Trigger & trigger)!=0)aEffect.TriggerEffect(containingSocket, targetCardSocket);
			}
		}

	}
}