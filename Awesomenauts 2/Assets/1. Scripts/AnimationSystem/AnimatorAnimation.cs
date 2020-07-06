using System.Collections;
using AwsomenautsCardGame.UI;
using UnityEngine;

namespace AwsomenautsCardGame.AnimationSystem
{
	[CreateAssetMenu(menuName = "Scriptable Objects/CardAnimation/AnimatorAnimation")]
	public class AnimatorAnimation : CardAnimation
	{
		public string AnimationName;
		public float AnimationDuration;

		public override IEnumerator Play(AnimationPlayer player, Transform target)
		{
			base.Play(player, target);

			if (player == null || player.Animator == null)
			{
				ExceptionViewUI.Instance.SetException(new UnityException("Animation " + AnimationName + " could not be triggered"));
			}

			player.Animator.Play(AnimationName);

			yield return new WaitForSeconds(AnimationDuration);
		}
	}
}