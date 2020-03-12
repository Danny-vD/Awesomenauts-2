using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Game Settings")]
public class GameSettingsObject : ScriptableObject
{
	public Card FatigueCard;
	public PlayerCardSettingsObject PlayerRedCardSettings;
	public PlayerCardSettingsObject PlayerBlueCardSettings;

	

	//Information that is global to the whole game
}
