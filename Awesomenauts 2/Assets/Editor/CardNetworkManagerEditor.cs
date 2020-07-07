using System;
using System.Linq;
using AwsomenautsCardGame.DataObjects.Networking;
using AwsomenautsCardGame.Enums.Cards;
using AwsomenautsCardGame.Gameplay.Cards;
using AwsomenautsCardGame.Networking;
using Mirror;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CardNetworkManager))]
public class CardNetworkManagerEditor : UnityEditor.Editor
{
	private CardNetworkManager src;

	private void OnEnable()
	{
		src = (CardNetworkManager) target;
	}

	public override void OnInspectorGUI()
	{
		EditorGUI.BeginChangeCheck();
		{
			Editor editor = CreateEditor(target, typeof(NetworkManagerEditor));
			editor.OnInspectorGUI();
		}

		if (EditorGUI.EndChangeCheck())
		{
			for (int i = 0; i < src.CardEntries.Length; i++)
			{
				CardEntry e = src.CardEntries[i];
				src.CardEntries[i] = Apply(e);
			}
		}
	}

	private static void SetStatName(EntityStatistics.InternalStat stat)
	{
		stat.name = $"{stat.type}[{stat.dataType}]: {stat.value}";
	}

	private CardEntry Apply(CardEntry e)
	{
		e.name = GetEntryName(e);

		for (int j = 0; j < e.Statistics.StartStatistics.Count; j++)
		{
			SetStatName(e.Statistics.StartStatistics[j]);
		}

		return e;
	}

	private static string GetEntryName(CardEntry e)
	{
		EntityStatistics.InternalStat stat = e.Statistics.StartStatistics.FirstOrDefault(x =>
			x.type == CardPlayerStatType.CardName);
		string s = stat == null ? "" : stat.value;
		return s;
	}
}