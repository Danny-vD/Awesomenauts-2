using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHand
{
	void AddCard(ICard card);
	bool CanAddCard();
	int GetCardCount();
}
