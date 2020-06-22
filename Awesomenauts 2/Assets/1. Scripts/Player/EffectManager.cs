using System.Collections.Generic;
using System.Text;
using Assets._1._Scripts.ScriptableObjects.Effects;
using Maps;

namespace Player
{
	public class EffectManager
	{
		public List<AEffect> Effects;

		public EffectManager(List<AEffect> effects)
		{
			Effects = effects;
		}

		public void TriggerEffects(EffectTrigger trigger, CardSocket containingSocket, CardSocket targetCardSocket,
			Card c = null)
		{
			for (int i = Effects.Count - 1; i >= 0; i--)
			{
                if(i >= Effects.Count) continue;
                AEffect aEffect = Effects[i];
				if ((aEffect.Trigger & trigger) != 0)
					aEffect.TriggerEffect(containingSocket == null ? c : containingSocket.DockedCard, containingSocket,
						targetCardSocket);
			}
		}

		public string GetEffectText()
		{
			StringBuilder sb = new StringBuilder();

			for (int i = 0; i < Effects.Count; i++)
			{
				sb.AppendLine(Effects[i].Description);
			}

			return sb.ToString();
		}
	}
}