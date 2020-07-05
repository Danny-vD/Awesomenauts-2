using System.Collections;
using UnityEngine;

namespace Assets._1._Scripts.AnimationSystem {
	public abstract class CardAnimation : ScriptableObject
	{
		public GameObject[] Particles;

		public virtual IEnumerator Play(AnimationPlayer player, Transform target)
		{
			CreateParticles();
			return null;
		}

		private void CreateParticles()
		{
			for (int i = 0; i < Particles.Length; i++)
			{
				GameObject p = Instantiate(Particles[i]);
			}
		}
	}
}