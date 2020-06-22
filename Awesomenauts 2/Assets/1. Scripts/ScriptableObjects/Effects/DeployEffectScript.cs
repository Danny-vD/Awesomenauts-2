using System.Collections;
using System.Collections.Generic;
using Assets._1._Scripts.ScriptableObjects.Effects;
using Maps;
using Player;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Effects/DeployEffect")]
public class DeployEffectScript : AEffect
{
	public override EffectTrigger Trigger => EffectTrigger.OnPlay;
	public AEffect Effect;

	public override IEnumerator TriggerEffect(Card c, CardSocket containingSocket, CardSocket targetSocket)
	{
		if (targetSocket.DockedCard.EffectManager.Effects.Contains(this))yield return null;
		targetSocket.DockedCard.EffectManager.Effects.Add(Effect);
		yield return null;
	}
}