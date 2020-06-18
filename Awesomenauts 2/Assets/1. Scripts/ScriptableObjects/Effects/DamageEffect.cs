using Maps;
using Player;
using UnityEngine;

namespace Assets._1._Scripts.ScriptableObjects.Effects
{
	[CreateAssetMenu(menuName = "Scriptable Objects/Effects/DamageEffect")]
	public class DamageEffect : AEffect
	{
		public override EffectTrigger Trigger => ETrigger;
		public EffectTrigger ETrigger = EffectTrigger.None;
		public int Damage;

		public override void TriggerEffect(Card c,CardSocket containingSocket, CardSocket targetSocket)
		{
			targetSocket.DockedCard.Statistics.SetValue(CardPlayerStatType.HP, targetSocket.DockedCard.Statistics.GetValue<int>(CardPlayerStatType.HP) - Damage);
		}
	}
}