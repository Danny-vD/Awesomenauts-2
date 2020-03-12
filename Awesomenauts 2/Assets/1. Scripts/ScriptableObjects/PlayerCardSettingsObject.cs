using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Player Card Settings")]
public class PlayerCardSettingsObject : ScriptableObject
{
	public float RotationOffset;
	public Card[] Cards;
	public LayerMask HandLayer;
	public CardDeck Deck;

	public CardHand Hand;
	//Information that is global to the whole game
}
