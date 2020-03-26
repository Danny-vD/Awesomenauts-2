using System;
using UnityEngine;

//using System.Collections.Generic;

[Serializable]
public struct CardInfo
{
	public string Name;
	public int CardRange;
	public int ATK;
	public int DEF;

	public Texture CardIcon;

	public CardDesign CardDesign;
	//public EffectTrigger EffectTrigger;
	//public List<Effect> Effects;
}
