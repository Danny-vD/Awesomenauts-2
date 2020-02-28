using System.Collections.Generic;
using UnityEngine;
using VDFramework;

namespace Character
{
	public class NautMovement : BetterMonoBehaviour
	{
		[SerializeField]
		private AnimationCurve movementCurve = null;

		[SerializeField, Range(0.001f, 1.0f)]
		private float drag = 0.02f;
		
		private CharacterController characterController;

		private Vector3 movementVector = Vector3.zero;

		private bool hasMoved;
		private float time;

		private void Awake()
		{
			characterController = GetComponent<CharacterController>();
		}
		
		private void Update()
		{
			if (!hasMoved)
			{
				time = 0;
				ApplyDrag();
			}
			
			hasMoved = false;
			
			ApplyGravity();
		}

		public void Move(Vector2 deltaMovement)
		{
			AddForce(deltaMovement.normalized * GetMovementSpeed());
			hasMoved = true;
		}

		private void AddForce(Vector2 force)
		{
			movementVector = force;
			characterController.Move(force * Time.deltaTime);
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

		private float GetMovementSpeed()
		{
			time += Time.deltaTime;
			return movementCurve.Evaluate(time);
		}
	}
}