using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDeck
{
	ICard DrawCard();
	int GetCardCount();
}
