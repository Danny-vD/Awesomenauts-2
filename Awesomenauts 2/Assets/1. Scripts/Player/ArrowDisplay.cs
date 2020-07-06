using System.Collections.Generic;
using UnityEngine;

namespace AwsomenautsCardGame.Player {

	public enum ArrowDisplayState {Default, Accept, Invalid, None}

	[RequireComponent(typeof(LineRenderer))]
	public class ArrowDisplay : MonoBehaviour
	{

		private LineRenderer lr;
		private void Start()
		{
			lr = GetComponent<LineRenderer>();
		}

		public Material DefaultMaterial;
		public Material InvalidMaterial;
		public Material AcceptMaterial;

		public bool UseSmoothing;

		[Header("Smoothing Settings")]
		public float TotalOffsetScale = 1;
		public float ArrowMaxOffset = 1;
		public float ArrowMaxOffsetRotation = 45;
		public AnimationCurve ArrowOffsetDistribution = AnimationCurve.EaseInOut(0, 0, 1, 1);
		public AnimationCurve ArrowRotationDistribution = AnimationCurve.EaseInOut(0, 0, 1, 1);
		public float PointDensityPerUnit = 4;

		public Vector3 OffsetDirection = Vector3.up;

		private Vector3[] dirtyPoints;

		private Vector3[] ProcessPoints(Vector3 start, Vector3 end)
		{
			if (!UseSmoothing) return new[] { start, end };
        
		

			List<Vector3> ret = new List<Vector3>();



			Vector3 delta = end - start;
			float deltaMag = delta.magnitude;
			float pointCount = PointDensityPerUnit * deltaMag;
			Vector3 deltaPerPoint = delta / pointCount;
			for (int i = 0; i < pointCount; i++)
			{
				Vector3 v = start + i * deltaPerPoint;
				float t = Mathf.Clamp01(i / pointCount);

				Vector3 offsetDir = OffsetDirection;
				float rotation = ArrowMaxOffsetRotation * ArrowRotationDistribution.Evaluate(t);
				Quaternion q = Quaternion.AngleAxis(rotation, delta);
				offsetDir = q * offsetDir;


				v += offsetDir * ArrowMaxOffset * ArrowOffsetDistribution.Evaluate(t);

			
				//Quaternion q = Quaternion.AngleAxis(rotation, delta);
				//v = q * v;
				//v *= TotalOffsetScale;
				ret.Add(v);
			}
			//float deltaPerPoint = deltaMag / pointCount;
			//for (int i = 0; i < pointCount; i++) //This is a for loop with a float.
			//{
			//	float lengthT = Mathf.Clamp01(i / pointCount);
			//	float offset = ArrowMaxOffset * ArrowOffsetDistribution.Evaluate(lengthT);
			//	float rotation = ArrowMaxOffsetRotation * ArrowRotationDistribution.Evaluate(lengthT);
			//	Vector3 v = start + delta * i * deltaPerPoint;
			//	v += perpendicular * offset;
			//	Quaternion q = Quaternion.AngleAxis(rotation, delta);
			//	v = q * v;
			//	v *= TotalOffsetScale;
			//	ret.Add(v);
			//}

			return ret.ToArray();
		}

		public void Deactivate()
		{
			lr.enabled = false;
		}

		public void SetArrowPositions(Vector3 start, Vector3 end, ArrowDisplayState state)
		{
			switch (state)
			{
				case ArrowDisplayState.Default:
					lr.sharedMaterial = DefaultMaterial;
					break;
				case ArrowDisplayState.Accept:
					lr.sharedMaterial = AcceptMaterial;
					break;
				case ArrowDisplayState.Invalid:
					lr.sharedMaterial = InvalidMaterial;
					break;
				case ArrowDisplayState.None:
					lr.enabled = false;
					return;
			}
			lr.enabled = true;
			Vector3[] positions = ProcessPoints(start, end);
			lr.positionCount = positions.Length;
			lr.SetPositions(positions);
		}
	}
}