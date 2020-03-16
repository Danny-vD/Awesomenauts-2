using UnityEngine;
using VDFramework;

namespace Character
{
	public class CharacterRotation : BetterMonoBehaviour
	{
		public void UpdateRotation(float deltaX)
		{
			if (deltaX < 0)
			{
				LookLeft();
			}

			if (deltaX > 0)
			{
				LookRight();
			}
		}

		private void LookLeft()
		{
			CachedTransform.rotation = Quaternion.LookRotation(Vector3.left, Vector3.up);
		}

		private void LookRight()
		{
			CachedTransform.rotation = Quaternion.LookRotation(Vector3.right, Vector3.up);
		}
	}
}