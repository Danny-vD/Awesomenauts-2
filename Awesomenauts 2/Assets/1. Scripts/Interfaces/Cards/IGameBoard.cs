using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameBoard
{
	bool BlueTurn { get; set; }
	IPlayer PlayerRed { get; }
	IPlayer PlayerBlue { get; }
	void Enqueue(IGameAction action);
	void Process();
    /// <summary>
	/// Can get used to get values from the game board.
	/// Can be useful to create different behaviour without scripting.
	/// </summary>
	/// <param name="type">The Value Type to be returned.</param>
	/// <returns></returns>
	int GetValue(GameBoardCardQueries type);
}
