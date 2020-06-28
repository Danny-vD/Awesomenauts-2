using System;

namespace Assets._1._Scripts.ScriptableObjects.Effects
{

	[Flags]
	public enum EffectTarget
	{
		TargetSocket = 1,
		Lane = 2,
		Player = 4,
		Enemies = 8,
		Allies = 16,
		Board = 32
	}
}