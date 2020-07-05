using System.Linq;
using Networking;
using Player;
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
				e.name = GetEntryName(e);
				scr.CardEntries[i] = e;
			}
		}
	}

	private string GetEntryName(CardEntry e)
	{
		EntityStatistics.InternalStat stat = e.Statistics.StartStatistics.FirstOrDefault(x =>
			x.type == CardPlayerStatType.CardName);
		string s = stat == null ? "" : stat.value;
		return s;
	}

}