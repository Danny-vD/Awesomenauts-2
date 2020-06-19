using Maps;
using Player;
using UnityEngine;

namespace Assets._1._Scripts.ScriptableObjects.Effects {


	[CreateAssetMenu(menuName = "Scriptable Objects/Effects/SuicideEffect")]
	public class SuicideEffect : AEffect
	{
		public override EffectTrigger Trigger => EffectTrigger.AfterAttacking;

		public override void TriggerEffect(Card c, CardSocket containingSocket, CardSocket targetSocket)
		{

			if (containingSocket.hasAuthority)
			{
				containingSocket.CmdUnDockCard();
			}
			else
			{
				containingSocket.DockCard(null);
			}


			Destroy(c.gameObject);
			base.TriggerEffect(c, containingSocket, targetSocket);
		}
	}
}