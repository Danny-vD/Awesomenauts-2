using System.Collections;
using AwsomenautsCardGame.Gameplay;
using AwsomenautsCardGame.Gameplay.Cards;
using UnityEngine;

namespace AwsomenautsCardGame.ScriptableObjects.Effects {
	[CreateAssetMenu(menuName = "Scriptable Objects/Effects/DeployEffect")]
	public class DeployEffectScript : AEffect
	{
		public override EffectTrigger Trigger => EffectTrigger.OnPlay;
		public AEffect Effect;
		public EffectTarget EffectTarget;

		public override IEnumerator TriggerEffect(Card c, CardSocket containingSocket, CardSocket targetSocket)
		{

			Card[] cards = AnimationEffectTargetLogic.GetTargets(EffectTarget, c, targetSocket);

			//Debug.Log("Deploying Effect: " + Effect.name + " on " + cards.Length + " Cards.");
			foreach (Card card in cards)
			{
				if (card.EffectManager.Effects.Contains(this)) continue;
				card.EffectManager.Effects.Add(Effect);
			}

			yield return null;
		}
	}
}