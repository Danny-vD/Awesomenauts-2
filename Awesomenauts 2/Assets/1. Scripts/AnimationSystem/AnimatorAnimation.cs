using System.Collections;
using UnityEngine;

namespace Assets._1._Scripts.AnimationSystem {
	[CreateAssetMenu(menuName = "Scriptable Objects/CardAnimation/AnimatorAnimation")]
	public class AnimatorAnimation : CardAnimation
	{
		public string AnimationName;
		public float AnimationDuration;

		public override IEnumerator Play(AnimationPlayer player, Transform target)
		{
			base.Play(player, target);
			player.Animator.Play(AnimationName);

			yield return new WaitForSeconds(AnimationDuration);
		}
	}
}