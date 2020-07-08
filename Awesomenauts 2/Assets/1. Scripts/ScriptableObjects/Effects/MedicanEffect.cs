using System;
using System.Collections;
using System.Linq;
using AwsomenautsCardGame.Enums.Cards;
using AwsomenautsCardGame.Gameplay;
using AwsomenautsCardGame.Gameplay.Cards;
using UnityEngine;

namespace AwsomenautsCardGame.ScriptableObjects.Effects
{
	[CreateAssetMenu(menuName = "Scriptable Objects/Effects/MedicanEffect")]
	public class MedicanEffect : AEffect
	{
		public override EffectTrigger Trigger => ETrigger;
		public EffectTrigger ETrigger = EffectTrigger.None;
		public CardPlayerStatType StatType => CardPlayerStatType.HP;

		public float Amount;
		public EffectTarget EffectTarget => EffectTarget.TargetSocket;

		void Awake()
		{

		}

		public override IEnumerator TriggerEffect(Card c, CardSocket containingSocket, CardSocket targetSocket)
		{

			EntityStatistics[] stats = AnimationEffectTargetLogic.GetTargets(EffectTarget, c, targetSocket).Select(x => x.Statistics).ToArray();
			//Debug.Log("Applying Effect: " + name + " on " + stats.Length + " Cards.");

			foreach (EntityStatistics stat in stats)
			{
				if (stat.HasValue(StatType) && stat.HasValue(CardPlayerStatType.MaxHP))
				{
					object val = stat.GetValue(StatType);
					Type t = val.GetType();

					float fval = val is float f ? f : (int)val;

					object val2 = stat.GetValue(CardPlayerStatType.MaxHP);
					float maxHP = val is float f2 ? f2 : (int)val2;



					fval = fval + maxHP * Amount;

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
				$"\n\tStatType: {StatType}\n\tEffectTarget: {EffectTarget}\n\tAmount: {Amount}";
			return ret;
		}
	}
}