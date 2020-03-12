using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEffect
{
	//TODO: Maybe needs more data than just game board
	bool CheckTrigger(IGameBoard board);
}
