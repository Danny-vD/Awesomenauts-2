using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICard
{
	//TODO
	int CardID { get; }
	CardInfo PlayerStatistics { get; }
	IEffect[] CardEffects { get; }
	Transform CardTransform { get; }
}
