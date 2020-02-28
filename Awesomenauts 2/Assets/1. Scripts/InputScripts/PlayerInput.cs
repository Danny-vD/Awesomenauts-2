using Character;
using UnityEngine;
using VDFramework;

namespace InputScripts
{
	[RequireComponent(typeof(CharacterMovement))]
	public class PlayerInput : BetterMonoBehaviour
	{
		private CharacterMovement nautMovement;

		private void Awake()
		{
			nautMovement = GetComponent<CharacterMovement>();
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