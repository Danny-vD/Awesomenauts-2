using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDeck
{
	void Initialize(ICard[] deckCards);
	ICard DrawCard();
	int GetCardCount();
}
