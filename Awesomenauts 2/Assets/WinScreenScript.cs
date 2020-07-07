using System.Collections;
using System.Collections.Generic;
using AwsomenautsCardGame.Gameplay.Cards;
using UnityEngine;
using UnityEngine.UI;

public class WinScreenScript : MonoBehaviour
{
	public Text Title;

	public Text Statistics;

	public Sprite[] TeamIDSprites;

	public Image Winner;

	public Image Loser;
	
	public void SetContent()
	{
		gameObject.SetActive(true);
		bool localWinner = CardPlayer.LastGame.LocalID == CardPlayer.LastGame.WinnerID;
		Title.text = localWinner ? "Win" : "Lose";
		Statistics.text = "Total Rounds: " + CardPlayer.LastGame.TotalRounds;
		Winner.sprite = TeamIDSprites[CardPlayer.LastGame.WinnerID];
		Loser.sprite = TeamIDSprites[CardPlayer.LastGame.LoserID];
	}
	
}
