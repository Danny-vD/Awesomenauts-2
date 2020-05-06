using System;

namespace Assets._1._Scripts.ScriptableObjects.DragLogic
{
	[Flags]
	public enum SocketTarget
	{
		OwnSockets = 1,
		EnemySockets = 2,
		NeutralSockets = 4,
		Empty = 8,
		Occupied = 16,
		AllSockets = -1,
	}
}