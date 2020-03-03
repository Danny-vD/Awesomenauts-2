using Animation;
using UnityEngine;
using VDFramework;

namespace Character
{
	[RequireComponent(typeof(CharacterMovement), typeof(NautAnimation))]
    public class Awesomenaut : BetterMonoBehaviour
    {
		[SerializeField]
		private Enums.Character.Awesomenaut nautName;

		public Enums.Character.Awesomenaut Name
		{
			get => nautName;
			private set => nautName = value;
		}
	}
}