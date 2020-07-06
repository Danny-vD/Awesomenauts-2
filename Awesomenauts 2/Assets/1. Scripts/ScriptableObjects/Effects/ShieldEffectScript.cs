using System.Collections;
using Assets._1._Scripts.ScriptableObjects.Effects;
using Maps;
using Player;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Effects/ShieldEffect")]
public class ShieldEffectScript : AEffect
{
	public override EffectTrigger Trigger => EffectTrigger.OnAttacked | EffectTrigger.OnAttacking;


	public override IEnumerator TriggerEffect(Card cardToProtect, CardSocket containingSocket, CardSocket targetSocket)
	{
		cardToProtect.Statistics.SetValue(CardPlayerStatType.HP,
			cardToProtect.Statistics.GetValue<int>(CardPlayerStatType.HP) +
			targetSocket.DockedCard.Statistics.GetValue<int>(CardPlayerStatType.Attack));


		cardToProtect.EffectManager.Effects.Remove(this);
		yield return null;
	}
}
