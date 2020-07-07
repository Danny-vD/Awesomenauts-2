using System;
using System.Text;
using AwsomenautsCardGame.Enums.Cards;
using AwsomenautsCardGame.Gameplay.Cards;
using AwsomenautsCardGame.ScriptableObjects.Effects;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Card))]
public class CardEditorScript : UnityEditor.Editor
{
	private Card scr;
	void OnEnable()
	{
		scr = (Card)target;
	}

	public override void OnInspectorGUI()
	{

		base.OnInspectorGUI();


		StringBuilder sb = new StringBuilder();
		sb.AppendLine(
			$"Entity Name: {(scr.Statistics.HasValue(CardPlayerStatType.CardName) ? scr.Statistics.GetValue<string>(CardPlayerStatType.CardName) : "NoName")}");

		sb.AppendLine(
			$"Entity Type: { (CardType)(scr.Statistics.HasValue(CardPlayerStatType.CardType) ? scr.Statistics.GetValue<int>(CardPlayerStatType.CardType) : 0)}");

		sb.AppendLine(
			$"Entity Owner ID: { (CardType)(scr.Statistics.HasValue(CardPlayerStatType.TeamID) ? scr.Statistics.GetValue<int>(CardPlayerStatType.TeamID) : -1)}");

		sb.AppendLine(
			$"Entity Solar: { (scr.Statistics.HasValue(CardPlayerStatType.Solar) ? scr.Statistics.GetValue<int>(CardPlayerStatType.Solar) : 0)}");

		sb.AppendLine(
			$"Entity HP: { (scr.Statistics.HasValue(CardPlayerStatType.HP) ? scr.Statistics.GetValue<int>(CardPlayerStatType.HP) : 0)}");

		sb.AppendLine(
			$"Entity Attack: { (scr.Statistics.HasValue(CardPlayerStatType.Attack) ? scr.Statistics.GetValue<int>(CardPlayerStatType.Attack) : 0)}");

		sb.AppendLine(
			$"Entity Range: { (scr.Statistics.HasValue(CardPlayerStatType.Range) ? scr.Statistics.GetValue<int>(CardPlayerStatType.Range) : 0)}");

		sb.AppendLine(
			$"Entity XRange: { (scr.Statistics.HasValue(CardPlayerStatType.CrossLaneRange) ? scr.Statistics.GetValue<int>(CardPlayerStatType.CrossLaneRange) : 0)}");

		sb.AppendLine(string.Empty);

		if (scr.EffectManager != null && scr.EffectManager.Effects != null)
		{

			sb.AppendLine("Effects:");
			foreach (AEffect effectManagerEffect in scr.EffectManager.Effects)
			{
				sb.AppendLine(effectManagerEffect.ToString());
			}
		}

		GUILayout.TextArea(sb.ToString());

	}

}
[CustomEditor(typeof(CardPlayer))]
public class CardPlayerEditorScript : UnityEditor.Editor
{
	private CardPlayer player;
	void OnEnable()
	{
		player = (CardPlayer)target;
	}

	public override void OnInspectorGUI()
	{

		base.OnInspectorGUI();

		Card scr = player.Awsomenaut;

		StringBuilder sb = new StringBuilder();
		if (scr == null)
		{
			GUILayout.TextArea("No Awsomenaut Attached");
			return;
		}

		try
		{
			sb.AppendLine(
				$"Client ID: {player.ClientID}");

			sb.AppendLine(
				$"Entity Name: {(scr.Statistics.HasValue(CardPlayerStatType.CardName) ? scr.Statistics.GetValue<string>(CardPlayerStatType.CardName) : "NoName")}");

			sb.AppendLine(
				$"Entity Owner ID: { (scr.Statistics.HasValue(CardPlayerStatType.TeamID) ? scr.Statistics.GetValue<int>(CardPlayerStatType.TeamID) : -1)}");
			
			sb.AppendLine(
				$"Entity Type: { (CardType)(scr.Statistics.HasValue(CardPlayerStatType.CardType) ? scr.Statistics.GetValue<int>(CardPlayerStatType.CardType) : 0)}");

			sb.AppendLine(
				$"Entity Solar: { (scr.Statistics.HasValue(CardPlayerStatType.Solar) ? scr.Statistics.GetValue<int>(CardPlayerStatType.Solar) : 0)}");

			sb.AppendLine(
				$"Entity HP: { (scr.Statistics.HasValue(CardPlayerStatType.HP) ? scr.Statistics.GetValue<int>(CardPlayerStatType.HP) : 0)}");

			sb.AppendLine(
				$"Entity Attack: { (scr.Statistics.HasValue(CardPlayerStatType.Attack) ? scr.Statistics.GetValue<int>(CardPlayerStatType.Attack) : 0)}");

			sb.AppendLine(
				$"Entity Range: { (scr.Statistics.HasValue(CardPlayerStatType.Range) ? scr.Statistics.GetValue<int>(CardPlayerStatType.Range) : 0)}");

			sb.AppendLine(
				$"Entity XRange: { (scr.Statistics.HasValue(CardPlayerStatType.CrossLaneRange) ? scr.Statistics.GetValue<int>(CardPlayerStatType.CrossLaneRange) : 0)}");


		}
		catch
		{
			sb.AppendLine(
				$"Client ID: {player.ClientID}");

			sb.AppendLine("Can not receive data for card.");
			Debug.LogWarning(sb.ToString(), scr);
		}

		
		GUILayout.TextArea(sb.ToString());

	}

}