namespace Interfaces
{
	public interface IDamageable
	{
		short MaxHealth { get; }
		short Health { get; }
		
		void Damage(short damageDealt);
	}
}