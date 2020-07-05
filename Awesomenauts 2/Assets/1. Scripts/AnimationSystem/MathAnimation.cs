using System.Collections;
using Assets._1._Scripts.ScriptableObjects.Effects;
using Player;
using UnityEngine;

namespace Assets._1._Scripts.AnimationSystem
{
	[CreateAssetMenu(menuName = "Scriptable Objects/CardAnimation/MathAnimation")]
	public class MathAnimation : CardAnimation
	{
		public float AnimationDuration;
		public AnimationCurve AnimationSpeed;
		public float AnimationXIntensity;
		public AnimationCurve AnimationXCurve;
		public float AnimationYIntensity;
		public AnimationCurve AnimationYCurve;
		public bool ResetToInitialPosition;
		

		public override IEnumerator Play(AnimationPlayer player, Transform target)
		{

			base.Play(player, target);

			Vector3 initPos = player.transform.position;
			Quaternion initRot = player.transform.rotation;
			float t = Time.realtimeSinceStartup;
			float targetTime = t + AnimationDuration;
			while (t <= targetTime)
			{
				t = Time.realtimeSinceStartup;

				float p = 1 - (targetTime - t) / AnimationDuration;
				p = AnimationSpeed.Evaluate(p) * p;

				Vector3 current = Vector3.Lerp(initPos, target.position, p);

				Vector3 localOff = new Vector3(AnimationXCurve.Evaluate(p) * AnimationXIntensity, AnimationYCurve.Evaluate(p) * AnimationYIntensity);

				player.transform.position = current;

				Vector3 dir = target.position - initPos;
				dir.y = 0;
				Quaternion localRot = Quaternion.FromToRotation(Vector3.back, dir);
				localOff = localRot * localOff;
				localOff.y = Mathf.Abs(localOff.y);
				player.transform.position += localOff;

				yield return new WaitForEndOfFrame();
			}

			if (ResetToInitialPosition)
			{
				player.transform.position = initPos;
				player.transform.rotation = initRot;
			}

			yield return null;
		}
	}
}