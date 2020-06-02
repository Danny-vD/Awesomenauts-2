using System.Collections;
using System.Collections.Generic;
using Maps;
using UnityEngine;

namespace Assets._1._Scripts.ScriptableObjects.Effects
{
	[CreateAssetMenu(menuName = "Scriptable Objects/Effects/OverTimeEffect")]
	public class OverTimeEffect : AEffect
	{
		public override EffectTrigger Trigger => EffectTrigger.OnRoundStart;
		public int Rounds;
		public AEffect Effect;

		public override void TriggerEffect(CardSocket containingSocket, CardSocket targetSocket)
		{
			if (Rounds > 0)
			{
				Rounds--;
				Effect.TriggerEffect(containingSocket, targetSocket);
			}
		}

	}
}