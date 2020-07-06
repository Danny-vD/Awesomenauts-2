using System;
using System.Collections;
using System.Linq;
using Maps;
using Player;
using UnityEngine;

namespace Assets._1._Scripts.ScriptableObjects.Effects
{
	[CreateAssetMenu(menuName = "Scriptable Objects/Effects/StatModifierEffect")]
	public class StatModifierEffect : AEffect
	{
		public override EffectTrigger Trigger => ETrigger;
		public EffectTrigger ETrigger = EffectTrigger.None;
		public CardPlayerStatType StatType;
		public float Amount;
		public bool Multiply;
		public EffectTarget EffectTarget;

		public override IEnumerator TriggerEffect(Card c, CardSocket containingSocket, CardSocket targetSocket)
		{

			EntityStatistics[] stats = AnimationEffectTargetLogic.GetTargets(EffectTarget, c, targetSocket).Select(x => x.Statistics).ToArray();
			//Debug.Log("Applying Effect: " + name + " on " + stats.Length + " Cards.");

			foreach (EntityStatistics stat in stats)
			{
				if (stat.HasValue(StatType))
				{
					object val = stat.GetValue(StatType);
					Type t = val.GetType();

					float fval = val is float f ? f : (int)val;



					fval = Multiply ? fval * Amount : fval + Amount;

					stat.SetValue(StatType, Convert.ChangeType(fval, t));
					//base.TriggerEffect(c, containingSocket, targetSocket);
				}
			}
			yield return null;
		}

		public override string ToString()
		{
			string ret = base.ToString();
			ret +=
				$"\n\tStatType: {StatType}\n\tEffectTarget: {EffectTarget}\n\tAmount: {Amount}\n\tMultiply: {Multiply}";
			return ret;
		}
	}
}