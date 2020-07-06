using System.Collections;
using AwsomenautsCardGame.Gameplay;
using AwsomenautsCardGame.Gameplay.Cards;
using UnityEngine;

namespace AwsomenautsCardGame.ScriptableObjects.Effects
{


	[CreateAssetMenu(menuName = "Scriptable Objects/Effects/DummyEffect")]
	public class DummyEffect : AEffect
	{
		public override EffectTrigger Trigger => TargetTrigger;
		public EffectTrigger TargetTrigger = EffectTrigger.OnAttacked;

		public override IEnumerator TriggerEffect(Card c, CardSocket containingSocket, CardSocket targetSocket)
		{
			yield return null; //Do Nothing
		}
	}
}