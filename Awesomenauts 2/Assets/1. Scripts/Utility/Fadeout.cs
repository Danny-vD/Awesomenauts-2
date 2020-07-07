using System;
using UnityEngine;
using VDFramework.VDUnityFramework.BaseClasses;

namespace AwsomenautsCardGame.Utility 
{
	public class Fadeout : BetterMonoBehaviour
	{
		[SerializeField]
		private Color color;
		
		[SerializeField, Range(0, 1)]
		private float transparency;

		private Material fadeoutMaterial;
		
		private static readonly int colorID = Shader.PropertyToID("_Color");
		private static readonly int alphaID = Shader.PropertyToID("_Alpha");

		private void Awake()
		{
			fadeoutMaterial = GetComponent<MeshRenderer>().sharedMaterial;
		}

		private void Update()
		{
			fadeoutMaterial.SetColor(colorID, color);
			fadeoutMaterial.SetFloat(alphaID, transparency);
		}
	}
}
