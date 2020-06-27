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

			EntityStatistics[] stats = null;
			if (EffectTarget == EffectTarget.Player)
			{
				stats = new[]{CardPlayer
					.ServerPlayers[
						MapTransformInfo.Instance.SocketManager.TID2CID(
							c.Statistics.GetValue<int>(CardPlayerStatType.TeamID))].PlayerStatistics};
			}
			else if (EffectTarget == EffectTarget.TargetSocket)
			{
				stats = new[] { targetSocket.DockedCard.Statistics };
			}
			else if ((EffectTarget & EffectTarget.Lane) != 0)
			{
				SocketSide side = targetSocket.SocketSide & (SocketSide.SideA | SocketSide.SideB); //Clean all other sides(red/blue/...)
				if ((EffectTarget & EffectTarget.Enemies) != 0)
				{
					stats = MapTransformInfo.Instance.SocketManager.GetSocketsOnSide(side).Where(x => x.HasCard && c.Statistics.GetValue<int>(CardPlayerStatType.TeamID) != x.DockedCard.Statistics.GetValue<int>(CardPlayerStatType.TeamID)).Select(x => x.DockedCard.Statistics).ToArray();

				}
				else if ((EffectTarget & EffectTarget.Allies) != 0)
				{
					stats = MapTransformInfo.Instance.SocketManager.GetSocketsOnSide(side).Where(x => x.HasCard && c.Statistics.GetValue<int>(CardPlayerStatType.TeamID) == x.DockedCard.Statistics.GetValue<int>(CardPlayerStatType.TeamID)).Select(x => x.DockedCard.Statistics).ToArray();
				}
			}
			else if ((EffectTarget & EffectTarget.Board) != 0)
			{
				if ((EffectTarget & EffectTarget.Enemies) != 0)
				{
					stats = MapTransformInfo.Instance.SocketManager.GetCardSockets().Where(x => x.HasCard && c.Statistics.GetValue<int>(CardPlayerStatType.TeamID) != x.DockedCard.Statistics.GetValue<int>(CardPlayerStatType.TeamID)).Select(x => x.DockedCard.Statistics).ToArray();
				}
				else if ((EffectTarget & EffectTarget.Allies) != 0)
				{
					stats = MapTransformInfo.Instance.SocketManager.GetCardSockets().Where(x => x.HasCard && c.Statistics.GetValue<int>(CardPlayerStatType.TeamID) == x.DockedCard.Statistics.GetValue<int>(CardPlayerStatType.TeamID)).Select(x => x.DockedCard.Statistics).ToArray();
				}
			}
			else
			{
				throw new Exception("No effect Target Specified in object: " + name);
			}

			Debug.Log("Applying Effect: " + name + " on " + stats.Length + " Cards.");

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

	}
}