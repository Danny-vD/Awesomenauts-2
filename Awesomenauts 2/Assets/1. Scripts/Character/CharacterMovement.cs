using Animation;
using UnityEngine;
using VDFramework;
using VDFramework.UnityExtensions;

namespace Character
{
	public class CharacterMovement : BetterMonoBehaviour
	{
		[SerializeField]
		private float maxSpeed = 8.0f;

		[SerializeField]
		private float timeForMaxSpeed = 0.5f;

		[SerializeField]
		private bool hasGravity = true;

		private readonly AnimationCurve movementCurve = new AnimationCurve();

		private Vector3 movementVector = Vector3.zero;

		private bool hasMoved;
		
		private float movementTime;
		private float dragTime;

		private CharacterController characterController;
		private CharacterRotation rotation;
		private NautAnimation nautAnimation;

		private void Awake()
		{
			characterController = GetComponent<CharacterController>();
			nautAnimation = GetComponent<NautAnimation>();

			rotation = this.EnsureComponent<CharacterRotation>();

			movementCurve.AddKey(0, 0);
			movementCurve.AddKey(timeForMaxSpeed, maxSpeed);
		}

		private void Update()
		{
			if (!hasMoved)
			{
				movementTime = 0;
				ApplyDrag();
			}
			else
			{
				dragTime = 0;
			}

			hasMoved = false;

			if (hasGravity)
			{
				ApplyGravity();
			}

			rotation.UpdateRotation(movementVector.x);
		}

		public void Move(Vector2 deltaMovement)
		{
			movementVector = deltaMovement.normalized * GetMovementSpeed();
			AddForce(movementVector);

			nautAnimation.Walk();
			hasMoved = true;
		}

		private void DragMove(Vector2 deltaMovement)
		{
			AddForce(deltaMovement.normalized * GetDragSpeed());
			nautAnimation.Drag();

			if (dragTime >= 1)
			{
				movementVector = Vector3.zero;
				nautAnimation.Idle();
			}
			else
			{
				nautAnimation.Drag();
			}
		}

		private void AddForce(Vector2 force)
		{
			characterController.Move(force * Time.deltaTime);
		}

		private void ApplyDrag()
		{
			DragMove(movementVector);
		}

		private void ApplyGravity()
		{
			characterController.Move(Physics.gravity * Time.deltaTime);
		}

		private float GetMovementSpeed()
		{
			movementTime += Time.deltaTime;
			return movementCurve.Evaluate(movementTime);
		}

		private float GetDragSpeed()
		{
			dragTime += Time.deltaTime;

			return movementCurve.Evaluate(timeForMaxSpeed - dragTime);
		}
	}
}