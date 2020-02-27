using Character;
using UnityEngine;
using VDFramework;

namespace InputScripts
{
	[RequireComponent(typeof(NautMovement))]
	public class PlayerInput : BetterMonoBehaviour
	{
		private NautMovement nautMovement;

		private void Awake()
		{
			nautMovement = GetComponent<NautMovement>();
		}

		private void Update()
		{
			if (Input.GetKey(KeyCode.D))
			{
				nautMovement.Move(Vector2.right);
			}
			else if (Input.GetKey(KeyCode.A))
			{
				nautMovement.Move(Vector2.left);
			}
		}
	}
}