using Maps;
using Player;
using UnityEngine;

namespace Assets._1._Scripts.ScriptableObjects.Effects {


	[CreateAssetMenu(menuName = "Scriptable Objects/Effects/SuicideEffect")]
	public class SuicideEffect : AEffect
	{
		public override EffectTrigger Trigger => EffectTrigger.AfterAttacking;

		public override void TriggerEffect(CardSocket containingSocket, CardSocket targetSocket)
		{
			Card c = containingSocket.DockedCard;
			containingSocket.DockCard(null);
			Destroy(c.gameObject);
		}
	}
}