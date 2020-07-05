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

			if (player.Animator == null)
			{
				ExceptionViewUI.Instance.SetException(new UnityException("Animation "+ AnimationName + " could not be triggered"));
			}

			player.Animator.Play(AnimationName);

			yield return new WaitForSeconds(AnimationDuration);
		}
	}
}