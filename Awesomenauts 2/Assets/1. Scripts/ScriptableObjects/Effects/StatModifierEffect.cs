using System;
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
		public bool ApplyToPlayer;

		public override void TriggerEffect(Card c ,CardSocket containingSocket, CardSocket targetSocket)
		{
			EntityStatistics stat = ApplyToPlayer
				? CardPlayer.ServerPlayers[MapTransformInfo.Instance.SocketManager.TID2CID(c.Statistics.GetValue<int>(CardPlayerStatType.TeamID))].PlayerStatistics
				: targetSocket.DockedCard.Statistics;


			if (stat.HasValue(StatType))
			{
				object val = stat.GetValue(StatType);
				Type t = val.GetType();

				float fval = val is float f ? f : (int)val;



				fval = Multiply ? fval * Amount : fval + Amount;

				stat.SetValue(StatType, Convert.ChangeType(fval, t));
				base.TriggerEffect(c, containingSocket, targetSocket);


			}
		}

	}
}