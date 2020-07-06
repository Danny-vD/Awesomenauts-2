using System;
using System.Collections;
using UnityEngine;

namespace Assets._1._Scripts.AnimationSystem
{
	[Flags]
	public enum MathAnimationFilter
	{
		Position = 1,
		Scale = 2,
	}

	[CreateAssetMenu(menuName = "Scriptable Objects/CardAnimation/MathAnimation")]
	public class MathAnimation : CardAnimation
	{
		public MathAnimationFilter Filter = MathAnimationFilter.Position | MathAnimationFilter.Scale;
		private bool UsePosition => (Filter & MathAnimationFilter.Position) != 0;
		private bool UseScale => (Filter & MathAnimationFilter.Scale) != 0;


		public float AnimationDuration;
		public AnimationCurve AnimationSpeed;
		public float AnimationXIntensity;
		public AnimationCurve AnimationXCurve;
		public float AnimationYIntensity;
		public AnimationCurve AnimationYCurve;

		public AnimationCurve ScaleAnimationCurveX = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));
		public AnimationCurve ScaleAnimationCurveY = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));
		public AnimationCurve ScaleAnimationCurveZ = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));

		public bool ResetToInitialPosition;


		public override IEnumerator Play(AnimationPlayer player, Transform target)
		{

			base.Play(player, target);

			Vector3 initPos = player.transform.position;
			Quaternion initRot = player.transform.rotation;
			Vector3 initScale = player.transform.localScale;
			float t = Time.realtimeSinceStartup;
			float targetTime = t + AnimationDuration;
			while (t <= targetTime)
			{
				t = Time.realtimeSinceStartup;

				float p = 1 - (targetTime - t) / AnimationDuration;
				p = AnimationSpeed.Evaluate(p) * p;

				Vector3 current = Vector3.Lerp(initPos, target.position, p);

				Vector3 localOff = new Vector3(AnimationXCurve.Evaluate(p) * AnimationXIntensity, AnimationYCurve.Evaluate(p) * AnimationYIntensity);

				if (UsePosition)
				{
					player.transform.position = current;
				}

				Vector3 dir = target.position - initPos;
				dir.y = 0;
				Quaternion localRot = Quaternion.FromToRotation(Vector3.back, dir);
				localOff = localRot * localOff;
				localOff.y = Mathf.Abs(localOff.y);
				player.transform.position += localOff;

				Vector3 scaleAnim = new Vector3(ScaleAnimationCurveX.Evaluate(p), ScaleAnimationCurveY.Evaluate(p), ScaleAnimationCurveZ.Evaluate(p));
				Vector3 localScale = new Vector3(initScale.x * scaleAnim.x, initScale.y * scaleAnim.y, initScale.z * scaleAnim.z);
				if (UseScale)
				{
					player.transform.localScale = localScale;
				}

				yield return new WaitForEndOfFrame();
			}

			if (ResetToInitialPosition)
			{
				player.transform.position = initPos;
				player.transform.rotation = initRot;
				player.transform.localScale = initScale;
			}

			yield return null;
		}
	}
}