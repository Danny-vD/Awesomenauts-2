using UnityEngine;
using VDFramework;

namespace Character
{
	[RequireComponent(typeof(NautMovement))]
    public class Awesomenaut : BetterMonoBehaviour
    {
		[SerializeField]
		private string nautName;

		public string Name
		{
			get => nautName;
			private set => nautName = value;
		}

		//private List<Ability> abilities = new List<Ability>();
	}
}