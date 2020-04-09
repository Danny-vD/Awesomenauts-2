﻿﻿using UnityEngine;

 namespace VDFramework.VDUnityFramework.UnityExtensions
{
	public static class TransformExtensions
	{
		/// <summary>
		/// Destroys all children
		/// </summary>
		public static void DestroyChildren(this Transform transform)
		{
			foreach (Transform child in transform)
			{
				Object.Destroy(child.gameObject);
			}
		}
	}
}