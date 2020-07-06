using UnityEngine;

namespace AwsomenautsCardGame.Gameplay {
	public class CameraController : MonoBehaviour
	{
		private bool init;
		private bool invertControls;
		private float currentScroll;
		private Vector2 currentMove;
		private Vector3 originalPosition;

		public void SetOriginalPosition(Vector3 pos, bool invert)
		{
			originalPosition = pos;
			invertControls = invert;
			if (invert)
			{

				MaxPlanarMove.position += InversionOffset;

			}

			init = true;
		}

		private Camera PlayerCamera;
		public float MaxZoom = 5f;
		public float ZoomSpeed = 1f;

		public Rect MaxPlanarMove = new Rect(Vector2.one * -10, Vector2.one * 20);
		public Vector2 InversionOffset;
		public float MoveSpeed = 1;
		// Start is called before the first frame update
		private void Start()
		{
			PlayerCamera = Camera.main;
		}

		// Update is called once per frame
		private void Update()
		{
			if (!init) return;
			float scroll = -Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed;
			ApplyScroll(scroll);

			Vector2 delta = new Vector2(-Input.GetAxis("Vertical"), Input.GetAxis("Horizontal")) * MoveSpeed;
			delta = invertControls ? delta * -1 : delta;
			ApplyPlanarMovement(new Vector2(delta.x, 0));
			ApplyPlanarMovement(new Vector2(0, delta.y));
		}

		private void ApplyPlanarMovement(Vector2 delta)
		{
			Vector2 newMove = currentMove + delta;
			//if (new Rect(-MaxPlaneMove, MaxPlaneMove).Contains(newMove, true))
			if (newMove.x > MaxPlanarMove.min.x && newMove.x < MaxPlanarMove.max.x &&
			    newMove.y > MaxPlanarMove.min.y && newMove.y < MaxPlanarMove.max.y)
			{
				transform.position = originalPosition + new Vector3(newMove.x, 0, newMove.y);
				currentMove = newMove;
			}
			//if (Mathf.Abs(newMove.x) < MaxPlaneMove.x && Mathf.Abs(newMove.y) < MaxPlaneMove.y)
			//{
			
			//}
		}

		private void ApplyScroll(float scroll)
		{
			if (scroll == 0) return;

			float s = currentScroll + scroll;
			if (s > 0 && s < MaxZoom)
			{
				currentScroll += scroll;
				originalPosition += PlayerCamera.transform.forward * scroll;
			}
		}
	}
}
