using System.Linq;
using AwsomenautsCardGame.Networking;
using AwsomenautsCardGame.Player;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CardNetworkManager))]
public class CardNetworkManagerEditor : UnityEditor.Editor
{

	public override void OnInspectorGUI()
	{
		CardNetworkManager scr = (CardNetworkManager)target;
		base.OnInspectorGUI();
		if (GUILayout.Button("Reload Card Entry Names"))
		{
			for (int i = 0; i < scr.CardEntries.Length; i++)
			{
				CardEntry e = scr.CardEntries[i];
				scr.CardEntries[i] = Apply(e);
			}
		}
	}

	private void SetStatName(EntityStatistics.InternalStat stat)
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

	private string GetEntryName(CardEntry e)
	{
		EntityStatistics.InternalStat stat = e.Statistics.StartStatistics.FirstOrDefault(x =>
			x.type == CardPlayerStatType.CardName);
		string s = stat == null ? "" : stat.value;
		return s;
	}

}