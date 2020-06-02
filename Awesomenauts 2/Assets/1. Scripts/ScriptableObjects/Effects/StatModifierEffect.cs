using System;
using Maps;
using Player;
using UnityEngine;

namespace Assets._1._Scripts.ScriptableObjects.Effects {
	[CreateAssetMenu(menuName = "Scriptable Objects/Effects/StatModifierEffect")]
	public class StatModifierEffect : AEffect
	{
		public override EffectTrigger Trigger => ETrigger;
		public EffectTrigger ETrigger = EffectTrigger.None;
		public CardPlayerStatType StatType;
		public float Amount;
		public bool Multiply;

		public override void TriggerEffect(CardSocket containingSocket, CardSocket targetSocket)
		{
			if (targetSocket.DockedCard.Statistics.HasValue(StatType))
			{
				object val = targetSocket.DockedCard.Statistics.GetValue(StatType);
				Type t = val.GetType();

				float fval = val is float f ? f : (int) val;



				fval = Multiply ? fval * Amount : fval + Amount;
				targetSocket.DockedCard.Statistics.SetValue(StatType, Convert.ChangeType(fval, t));

			}
		}

	}
}