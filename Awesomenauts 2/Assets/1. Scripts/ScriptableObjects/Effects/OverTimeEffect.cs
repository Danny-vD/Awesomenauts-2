using System.Collections;
using AwsomenautsCardGame.Gameplay;
using AwsomenautsCardGame.Gameplay.Cards;
using UnityEngine;

namespace AwsomenautsCardGame.ScriptableObjects.Effects
{
	[CreateAssetMenu(menuName = "Scriptable Objects/Effects/OverTimeEffect")]
	public class OverTimeEffect : AEffect
	{
		public override EffectTrigger Trigger => EffectTrigger.OnRoundStart;
		public int Rounds;
		public AEffect Effect;

		public override IEnumerator TriggerEffect(Card c, CardSocket containingSocket, CardSocket targetSocket)
		{
			if (Rounds > 0)
			{
				Rounds--;

				yield return Effect.TriggerEffect(c, containingSocket, targetSocket);
				//base.TriggerEffect(c, containingSocket, targetSocket);
			}

			yield return null;
		}

	}
}