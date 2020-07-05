using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets._1._Scripts.AnimationSystem
{
	public class AnimationPlayer : MonoBehaviour
	{
		public List<CardAnimationEntry> Animations;
		public Animator Animator;

		private void Start()
		{
			if (Animator == null) Animator = GetComponent<Animator>();
		}

		public IEnumerator Play(string animName, Transform target)
		{
			CardAnimationEntry anim = Animations.FirstOrDefault(x => x.name == animName);
			if (anim != null)
			{
				yield return Play(anim.animation, target);
			}
		}

		public IEnumerator Play(CardAnimation anim, Transform target)
		{
			yield return anim.Play(this, target);
		}
	}
}