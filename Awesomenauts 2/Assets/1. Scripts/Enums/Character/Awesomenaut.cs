using System;

namespace Enums.Character
{
	[Flags]
	public enum Awesomenaut
	{
		All = -1,
		SheriffLonestar = 1,
		Voltar = 2, //the Omniscient
		Scoop = 4,
		ProfessorYoolip = 8,
		Deadlift = 16,
	}
}