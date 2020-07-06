using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace AwsomenautsCardGame.UI.DebugPanel
{
	public class SolarDisplay : MonoBehaviour
	{
		public Text SolarText;
		public List<Image> SolarImages;
		public Sprite SolarActive;
		public Sprite SolarInactive;


		public enum AnimationMode { Sin, AbsSin, ModSin, AbsModSin, Custom }

		[Header("Animation")]
		public AnimationMode Mode;
		public float Speed;
		public float Multiplicator = 1;
		public float Delay;
		[HideInInspector]
		public float Modulus = 1;


		private int cost;
		private int displayedSolar;
		private bool resetFrame;

		public AnimationCurve CustomCurve;

		private static Dictionary<AnimationMode, Func<float, float, float, float>> AnimationModes => new Dictionary<AnimationMode, Func<float, float, float, float>>
		{
			{AnimationMode.Sin, (x,mod, mul) => Mathf.Sin(x) * mul },
			{AnimationMode.AbsSin, (x, mod, mul) => Mathf.Abs(Mathf.Sin(x))  * mul},
			{AnimationMode.ModSin, (x, mod, mul) => Mathf.Sin(x) % mod },
			{AnimationMode.AbsModSin, (x, mod, mul) => Mathf.Abs(Mathf.Sin(x)) % mod * mul },
			{AnimationMode.Custom, (x, mod, mul) =>
				DebugPanelInfo.Instance.SolarDisp.CustomCurve.Evaluate(x) * mul},
		};


		public void SetSolar(int amount)
		{
			displayedSolar = amount;
			if (SolarText != null)
				SolarText.text = $"{(int)amount}/10";
			if(SolarImages.All(x => x != null))
			{
				SolarImages.ForEach(x => x.sprite = SolarInactive);
				SolarImages.Take(amount).ToList().ForEach(x => x.sprite = SolarActive);
			}
		}

		public void SetSolarCost(int amount)
		{
			if (amount == 0) resetFrame = true;
			cost = amount;
		}

		public static float GetRotation(float addition, float speed, AnimationMode mode, float modulus, float multiplicator)
		{
			float time = Time.realtimeSinceStartup * speed + addition;
			float rot = AnimationModes[mode].Invoke(time, modulus, multiplicator);
			return rot;
		}

		private static void Animate(int displayedSolar, int cost, float delay, float speed, AnimationMode mode, float modulus, float multiplicator, List<Image> SolarImages, Text SolarText)
		{
			SolarText.text = $"{displayedSolar - cost}/10";

			int offset = displayedSolar - cost;
			for (int i = cost - 1; i >= 0; i--)
			{
				Image solarImage = SolarImages[i + offset];
				solarImage.transform.rotation = Quaternion.identity;
				if (i <= cost)
				{
					solarImage.transform.rotation = Quaternion.AngleAxis(GetRotation(i / Mathf.Max(cost, 1f) * delay, speed, mode, modulus, multiplicator), Vector3.back);
				}
			}
		}

		private void Update()
		{
			if (cost != 0 && SolarText != null)
			{
				Animate(displayedSolar, cost, Delay, Speed, Mode, Modulus, Multiplicator, SolarImages, SolarText);
			}

		}

		private void LateUpdate()
		{
			if (resetFrame)
			{
				resetFrame = false;
				SolarText.text = $"{(int)displayedSolar}/10";
				foreach (Image solarImage in SolarImages)
				{
					solarImage.transform.rotation = Quaternion.identity;
				}
			}
		}
	}

}