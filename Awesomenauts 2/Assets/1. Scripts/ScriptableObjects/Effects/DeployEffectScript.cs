using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets._1._Scripts.ScriptableObjects.Effects;
using Maps;
using Player;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Effects/DeployEffect")]
public class DeployEffectScript : AEffect
{
	public override EffectTrigger Trigger => EffectTrigger.OnPlay;
	public AEffect Effect;
	public EffectTarget EffectTarget;

	public override IEnumerator TriggerEffect(Card c, CardSocket containingSocket, CardSocket targetSocket)
	{

		Card[] cards = null;
		if (EffectTarget == EffectTarget.Player)
		{
			throw new Exception("Currently impossible to use the player as a target to deploy an effect: " + Effect.name);
			//TODO: When implementing the awsomenauts we need to add the awsomenaut as a card here
			//cards = new[]{CardPlayer
			//	.ServerPlayers[
			//		MapTransformInfo.Instance.SocketManager.TID2CID(
			//			c.Statistics.GetValue<int>(CardPlayerStatType.TeamID))].PlayerStatistics};
		}
		else if (EffectTarget == EffectTarget.TargetSocket)
		{
			cards = new[] { targetSocket.DockedCard };
		}
		else if ((EffectTarget & EffectTarget.Lane) != 0)
		{
			SocketSide side = targetSocket.SocketSide & (SocketSide.SideA | SocketSide.SideB); //Clean all other sides(red/blue/...)
			if ((EffectTarget & EffectTarget.Enemies) != 0)
			{
				cards = MapTransformInfo.Instance.SocketManager.GetSocketsOnSide(side).Where(x => x.HasCard && c.Statistics.GetValue<int>(CardPlayerStatType.TeamID) != x.DockedCard.Statistics.GetValue<int>(CardPlayerStatType.TeamID)).Select(x => x.DockedCard).ToArray();

			}
			else if ((EffectTarget & EffectTarget.Allies) != 0)
			{
				cards = MapTransformInfo.Instance.SocketManager.GetSocketsOnSide(side).Where(x => x.HasCard && c.Statistics.GetValue<int>(CardPlayerStatType.TeamID) == x.DockedCard.Statistics.GetValue<int>(CardPlayerStatType.TeamID)).Select(x => x.DockedCard).ToArray();
			}
		}
		else if ((EffectTarget & EffectTarget.Board) != 0)
		{
			if ((EffectTarget & EffectTarget.Enemies) != 0)
			{
				cards = MapTransformInfo.Instance.SocketManager.GetCardSockets().Where(x => x.HasCard && c.Statistics.GetValue<int>(CardPlayerStatType.TeamID) != x.DockedCard.Statistics.GetValue<int>(CardPlayerStatType.TeamID)).Select(x => x.DockedCard).ToArray();
			}
			else if((EffectTarget & EffectTarget.Allies) != 0)
			{
				cards = MapTransformInfo.Instance.SocketManager.GetCardSockets().Where(x => x.HasCard && c.Statistics.GetValue<int>(CardPlayerStatType.TeamID) == x.DockedCard.Statistics.GetValue<int>(CardPlayerStatType.TeamID)).Select(x => x.DockedCard).ToArray();
			}
		}
		else
		{
			throw new Exception("No effect Target Specified in object: " + name);
		}

		//Debug.Log("Deploying Effect: " + Effect.name + " on " + cards.Length + " Cards.");
		foreach (Card card in cards)
		{
			if (card.EffectManager.Effects.Contains(this)) continue;
			card.EffectManager.Effects.Add(Effect);
		}
		
		yield return null;
	}
}