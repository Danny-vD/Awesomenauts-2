using UnityEngine;
using VDFramework;

namespace CameraScript
{
	public class PlayerCamera : BetterMonoBehaviour
	{
		private const string cameraName = "PlayerCamera";
		
		private bool followPlayer = true;

		[SerializeField]
		private Vector3 offset;

		[SerializeField]
		private GameObject cameraPrefab;

		private GameObject cameraToMove;

		private void Awake()
		{
			cameraToMove = Instantiate(cameraPrefab);
			cameraToMove.name = cameraName;
			cameraToMove.hideFlags = HideFlags.HideInHierarchy;

			SetCameraOffset();
		}

		private void LateUpdate()
		{
			if (followPlayer)
			{
				SetCameraOffset();
			}
		}

		private void SetCameraOffset()
		{
			Vector3 position = CachedTransform.position;
			cameraToMove.transform.position = position + offset;
		}
	}
}