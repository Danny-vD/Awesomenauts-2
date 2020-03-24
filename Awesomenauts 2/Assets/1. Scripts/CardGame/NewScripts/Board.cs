using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
	public bool GameStarted { get; private set; }
	public int Turn { get; private set; }
	public int PlayerTurn => Turn % 2;
	public static Board Instance { get; private set; }
	public GameObject[] CardSockets;

	void Awake()
	{
		Instance = this;
	}

	public void StartGame()
	{
		GameStarted = true;
		Turn = 0;
	}

	public void EndTurn(int clientID)
	{
		if (clientID == PlayerTurn)
		{
			//Accept Player Requesting End Turn
			Turn++;
		}
	}



}
