using System.Collections;
using System.Collections.Generic;
using Maps;
using Player;
using UnityEngine;
namespace Assets._1._Scripts.ScriptableObjects.Effects
{

	[CreateAssetMenu(menuName = "Scriptable Objects/Effects/RetaliateEffect")]
	public class RetaliateEffect : AEffect
	{
		public override EffectTrigger Trigger => EffectTrigger.AfterAttacking;

		public override void TriggerEffect(Card c, CardSocket containingSocket, CardSocket targetSocket)
		{
			if (containingSocket != null && containingSocket.HasCard && containingSocket.DockedCard.Statistics.GetValue<int>(CardPlayerStatType.HP) > 0 &&
				targetSocket.DockedCard.Statistics.GetValue<int>(CardPlayerStatType.HP) > 0)
			{
				containingSocket.DockedCard.Attack(targetSocket.DockedCard);
				base.TriggerEffect(c, containingSocket, targetSocket);
			}
		}
	}

}