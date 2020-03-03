using UnityEngine;
using VDFramework;

namespace Animation
{
	[RequireComponent(typeof(Animator))]
	public class NautAnimation : BetterMonoBehaviour
	{
		private Animator animator;
		
		private static readonly int isWalking = Animator.StringToHash("IsWalking");
		private static readonly int drag = Animator.StringToHash("Drag");
		private static readonly int isFalling = Animator.StringToHash("IsFalling");
		private static readonly int isIdle = Animator.StringToHash("IsIdle");
		private static readonly int isTeleporting = Animator.StringToHash("IsTeleporting");
		private static readonly int isStunned = Animator.StringToHash("IsStunned");
		private static readonly int abilityStart = Animator.StringToHash("AbilityStart");
		private static readonly int abilityPerform = Animator.StringToHash("AbilityPerform");
		private static readonly int abilityEnd = Animator.StringToHash("AbilityEnd");

		private void Awake()
		{
			animator = GetComponent<Animator>();
		}

		public void Default()
		{
			Idle();
		}

		public void Idle()
		{
			SetTrigger(isIdle);
		}
		
		public void Walk()
		{
			SetTrigger(isWalking);
		}

		public void Drag()
		{
			SetTrigger(drag);
		}

		public void Fall()
		{
			SetTrigger(isFalling);
		}

		public void Teleport()
		{
			SetTrigger(isTeleporting);
		}

		public void Stunned()
		{
			SetTrigger(isStunned);
		}

		public void AbilityStart()
		{
			SetTrigger(abilityStart);
		}
		
		public void AbilityPerform()
		{
			SetTrigger(abilityPerform);
		}
		
		public void AbilityEnd()
		{
			SetTrigger(abilityEnd);
		}
		
		private void SetTrigger(int trigger)
		{
			animator.SetTrigger(trigger);
		}
	}
}