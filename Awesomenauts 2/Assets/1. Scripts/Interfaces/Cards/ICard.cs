using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICard
{
	//TODO
	object PlayerStatistics { get; }
	IEffect[] CardEffects { get; }
}
