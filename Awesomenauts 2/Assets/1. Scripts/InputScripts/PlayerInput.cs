using Audio;
using Character;
using Enums.Audio;
using Events.Audio;
using UnityEngine;
using VDFramework;
using VDFramework.EventSystem;
using Awesomenaut = Enums.Character.Awesomenaut;

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