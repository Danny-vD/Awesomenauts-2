using Animation;
using UnityEngine;
using VDFramework;
using VDFramework.UnityExtensions;

namespace Character
{
	public class CharacterMovement : BetterMonoBehaviour
	{
		[SerializeField]
		private AnimationCurve movementCurve = null;

		[SerializeField, Range(0.001f, 1.0f)]
		private float drag = 0.02f;
		
		private CharacterController characterController;
		private Vector3 movementVector = Vector3.zero;

		private bool hasMoved;
		private float time;

		private CharacterRotation rotation;
		private NautAnimation nautAnimation;

		private void Awake()
		{
			characterController = GetComponent<CharacterController>();
			nautAnimation = GetComponent<NautAnimation>();

			rotation = this.EnsureComponent<CharacterRotation>();
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
			
			rotation.UpdateRotation(movementVector.x);
		}

		public void Move(Vector2 deltaMovement)
		{
			AddForce(deltaMovement.normalized * GetMovementSpeed());
			nautAnimation.Walk();
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

			if (movementVector.magnitude < 0.15f)
			{
				movementVector = Vector3.zero;
				nautAnimation.Idle();
			}
			else
			{
				nautAnimation.Drag();
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