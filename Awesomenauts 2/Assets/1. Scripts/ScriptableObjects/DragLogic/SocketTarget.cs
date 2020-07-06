using System;

namespace AwsomenautsCardGame.ScriptableObjects.DragLogic
{
	[Flags]
	public enum SocketTarget
	{
		Nothing = 0,
		OwnSockets = 1,
		EnemySockets = 2,
		NeutralSockets = 4,
		Empty = 8,
		Occupied = 16,
		Turret = 32,
		AllSockets = -1,
	}
}