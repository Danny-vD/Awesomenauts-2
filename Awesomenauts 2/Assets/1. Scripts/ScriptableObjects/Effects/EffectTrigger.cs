using System;

namespace Assets._1._Scripts.ScriptableObjects.Effects {

	[Flags]
	public enum EffectTrigger
	{
		None = 0,
		OnDeath = 1,
		AfterPlay = 2,
		AfterAttacking = 4,
		AfterAttacked = 8,
		AfterMove = 16,
		AfterRoundEnd = 32,
		/// <summary>
		/// Unused, but kept to not break the scene settings
		/// </summary>
		AfterRoundStart = 64,
		OnPlay = 128,
		OnAttacking = 256,
		OnAttacked = 512,
		OnMove = 1024,
		/// <summary>
		/// Unused, but kept to not break the scene settings
		/// </summary>
		OnRoundEnd = 2048,
		OnRoundStart = 4096,
	}
}