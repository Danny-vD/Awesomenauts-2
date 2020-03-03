using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VDFramework;
using VDFramework.Extensions;

namespace Animation
{
	[RequireComponent(typeof(Animator))]
	public class NautAnimation : BetterMonoBehaviour
	{
		private enum State
		{
			IsWalking,
			Drag,
			IsFalling,
			IsIdle,
			IsTeleporting,
			IsStunned,
			AbilityStart,
			AbilityPerform,
			AbilityEnd,
		}
		
		private Animator animator;
		
		private static readonly Dictionary<State, int> triggerIDs = new Dictionary<State, int>();

		private void Awake()
		{
			animator = GetComponent<Animator>();

			foreach (State state in State.Drag.GetValues())
			{
				triggerIDs.Add(state, Animator.StringToHash(state.ToString()));
			}
		}

		public void Default()
		{
			Idle();
		}

		public void Idle()
		{
			SetTrigger(State.IsIdle);
		}
		
		public void Walk()
		{
			SetTrigger(State.IsWalking);
		}

		public void Drag()
		{
			SetTrigger(State.Drag);
		}

		public void Fall()
		{
			SetBool(State.IsFalling, true);
		}

		public void Grounded()
		{
			SetBool(State.IsFalling, false);
		}

		public void Teleport()
		{
			SetTrigger(State.IsTeleporting);
		}

		public void Stunned()
		{
			SetTrigger(State.IsStunned);
		}

		public void AbilityStart()
		{
			SetTrigger(State.AbilityStart);
		}
		
		public void AbilityPerform()
		{
			SetTrigger(State.AbilityPerform);
		}
		
		public void AbilityEnd()
		{
			SetTrigger(State.AbilityEnd);
		}
		
		private void SetTrigger(State trigger)
		{
			foreach (int id in triggerIDs.Values)
			{
				animator.ResetTrigger(id);
			}
			
			animator.SetTrigger(triggerIDs[trigger]);
		}

		private void SetBool(State state, bool value)
		{
			animator.SetBool(triggerIDs[state], value);
		}
	}
}