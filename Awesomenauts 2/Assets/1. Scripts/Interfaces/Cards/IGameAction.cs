using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameAction
{
	void Process(IGameBoard board);
}
