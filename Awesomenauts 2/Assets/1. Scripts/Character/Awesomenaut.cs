using VDFramework;

namespace Character
{
    public abstract class Awesomenaut : BetterMonoBehaviour
    {
		public uint MaxHealth { get; protected set; }
		
		public uint Health { get; protected set; }
		
		public string Name { get; protected set; }
    }
}