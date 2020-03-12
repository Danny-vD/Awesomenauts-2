using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum GameBoardCardQueries
{
	CardsOnBoard = 1,
	CardsDestroyed = 2,
	CardsPlayed = 4,

	CardTeamRed = 8,
	CardTeamBlue = 16,
	CardTeamNeutral = 32,
	CardTeamAll = CardTeamBlue | CardTeamRed | CardTeamNeutral,
	CardTeamNone = 0
	//More Queries can be added.
}
