using System.Collections.Generic;
using UnityEngine;
using VDFramework;

namespace Character
{
	public class NautMovement : BetterMonoBehaviour
	{
		[SerializeField]
		private float movementSpeed = 8.0f;

		[SerializeField, Range(0.001f, 1.0f)]
		private float drag = 0.02f;
		
		private CharacterController characterController;

		private Vector3 movementVector = Vector3.zero;

		private bool hasMoved;

		private void Awake()
		{
			characterController = GetComponent<CharacterController>();
		}
		
		private void Update()
		{
			if (!hasMoved)
			{
				ApplyDrag();
			}
			
			hasMoved = false;
			
			ApplyGravity();
		}

		public void Move(Vector2 deltaMovement)
		{
			AddForce(deltaMovement);
			hasMoved = true;
		}

		private void AddForce(Vector2 force)
		{
			float speed = movementSpeed;

			speed *= Time.deltaTime;

			if (force.magnitude > movementSpeed)
			{
				force.Normalize();
			}

			Vector3 velocity = force * speed;
			movementVector = force;

			characterController.Move(velocity);
		}

		private void ApplyDrag()
		{
			characterController.Move(movementVector * Time.deltaTime);

			movementVector *= 1 - drag;

			if (movementVector.magnitude < 0.1f)
			{
				movementVector = Vector3.zero;
			}
		}
		
		private void ApplyGravity()
		{
			characterController.Move(Physics.gravity * Time.deltaTime);
		}
	}
}