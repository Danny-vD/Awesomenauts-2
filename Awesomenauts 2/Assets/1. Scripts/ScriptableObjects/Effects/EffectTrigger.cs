namespace Assets._1._Scripts.ScriptableObjects.Effects {
	public enum EffectTrigger
	{
		None = 0,
		OnDeath = 1,
		AfterPlay = 2,
		AfterAttacking = 4,
		AfterAttacked = 8,
		AfterMove = 16,
		AfterRoundEnd = 32,
		AfterRoundStart = 64,
		OnPlay = 128,
		OnAttacking = 256,
		OnAttacked = 512,
		OnMove = 1024,
		OnRoundEnd = 2048,
		OnRoundStart = 4096,
	}
}