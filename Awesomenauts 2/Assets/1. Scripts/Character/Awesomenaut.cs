using UnityEngine;
using VDFramework;

namespace Character
{
	[RequireComponent(typeof(NautMovement))]
    public class Awesomenaut : BetterMonoBehaviour
    {
		[SerializeField]
		private Enums.Character.Awesomenaut nautName;

		public Enums.Character.Awesomenaut Name
		{
			get => nautName;
			private set => nautName = value;
		}

		//private List<Ability> abilities = new List<Ability>();
	}
}