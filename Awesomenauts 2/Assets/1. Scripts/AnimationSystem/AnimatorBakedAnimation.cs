using System.Collections;
using UnityEngine;

namespace Assets._1._Scripts.AnimationSystem {
	[CreateAssetMenu(menuName = "Scriptable Objects/CardAnimation/AnimatorBakedAnimation")]
	public class AnimatorBakedAnimation : CardAnimation
	{
		public string AnimationName;

		public override IEnumerator Play(AnimationPlayer player, Transform target)
		{
			base.Play(player, target);
			player.Play(AnimationName, target); //Call a animation from the Animation Player by name
			return null;
		}
	}
}