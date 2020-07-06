using UnityEngine;

namespace AwsomenautsCardGame.UI {
	public class ButtonOnClickAnimation : MonoBehaviour
	{

		public float ShakeTime = 0.5f;
		public float ShakeSpeed = 1f;
		public AnimationCurve ShakeIntensity;
		public float IntensityMultiplier = 1;
		private float t;
		private Vector3 originalPos;
		private bool animating;


		public void TriggerShake()
		{
			t = 0;
			originalPos = transform.position;
			animating = true;
		}

		// Update is called once per frame
		private void Update()
		{
			if (animating && t < ShakeTime)
			{
				float sample0 = t * ShakeSpeed;
				float sample1 = (ShakeTime - t) * ShakeSpeed;

				Vector3 pos = originalPos +
				              new Vector3(Mathf.PerlinNoise(sample0, sample0) * 2 - 1, Mathf.PerlinNoise(sample1, sample1) * 2 - 1, 0) * ShakeIntensity.Evaluate(t / ShakeTime) * IntensityMultiplier;
				transform.position = pos;
				t += Time.deltaTime;
			}
			else if (animating)
			{
				animating = false;
				transform.position = originalPos;
			}
		}
	}
}
