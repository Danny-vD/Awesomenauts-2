using System.Collections;
using AwsomenautsCardGame.Enums.Cards;
using AwsomenautsCardGame.Gameplay;
using AwsomenautsCardGame.Gameplay.Cards;
using UnityEngine;

namespace AwsomenautsCardGame.ScriptableObjects.Effects
{
	[CreateAssetMenu(menuName = "Scriptable Objects/Effects/DamageEffect")]
	public class DamageEffect : AEffect
	{
		public override EffectTrigger Trigger => ETrigger;
		public EffectTrigger ETrigger = EffectTrigger.None;
		public int Damage;

		public override IEnumerator TriggerEffect(Card c, CardSocket containingSocket, CardSocket targetSocket)
		{
			if (targetSocket.DockedCard != null)
				targetSocket.DockedCard.Statistics.SetValue(CardPlayerStatType.HP, targetSocket.DockedCard.Statistics.GetValue<int>(CardPlayerStatType.HP) - Damage);



			//base.TriggerEffect(c, containingSocket, targetSocket);
			yield return null;
		}
	}
}