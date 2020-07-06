using System.Collections;
using AwsomenautsCardGame.Enums.Cards;
using AwsomenautsCardGame.Gameplay;
using AwsomenautsCardGame.Gameplay.Cards;
using UnityEngine;

namespace AwsomenautsCardGame.ScriptableObjects.Effects {
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
}
