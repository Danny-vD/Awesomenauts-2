using System.Collections;
using System.Collections.Generic;
using Maps;
using Player;
using UnityEngine;

namespace Assets._1._Scripts.ScriptableObjects.Effects
{
	[CreateAssetMenu(menuName = "Scriptable Objects/Effects/OverTimeEffect")]
	public class OverTimeEffect : AEffect
	{
		public override EffectTrigger Trigger => EffectTrigger.OnRoundStart;
		public int Rounds;
		public AEffect Effect;

		public override void TriggerEffect(Card c, CardSocket containingSocket, CardSocket targetSocket)
		{
			if (Rounds > 0)
			{
				Rounds--;
				Effect.TriggerEffect(c,containingSocket, targetSocket);
				base.TriggerEffect(c, containingSocket, targetSocket);
			}
		}

	}
}