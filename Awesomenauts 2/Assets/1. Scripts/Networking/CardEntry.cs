using System;
using System.Collections.Generic;
using UnityEngine;

namespace Networking
{
	/// <summary>
	/// Info Object for a Card
	/// </summary>
	[Serializable]
	public struct CardEntry
	{
		public EntityStatistics Statistics;
		public GameObject Prefab;
		//Stats/Designs/etc

		public Tuple<int[], int[], string[]> StatisticsToNetworkableTypes()
		{

			Tuple<int[], int[], string[]> ret = 
				new Tuple<int[], int[], string[]>(
					new int[Statistics.Stats.Count],
					new int[Statistics.Stats.Count], 
					new string[Statistics.Stats.Count]);
			int i = 0;
			foreach (KeyValuePair<CardPlayerStatType, CardPlayerStat> statisticsStat in Statistics.Stats)
			{
				ret.Item1[i] = (int)statisticsStat.Key;
				ret.Item2[i] = (int)statisticsStat.Value.Type;
				object o = statisticsStat.Value.GetValue();
				ret.Item3[i] = o.ToString();
				i++;
			}

			return ret;
		}
		//int[] Stat Type Enum
		//int[] Stat Type
		//float[] Stat Values
		public static EntityStatistics FromNetwork(Tuple<int[], int[], string[]> stats)
		{
			EntityStatistics e = new EntityStatistics();
			e.InitializeStatDictionary(); //Creates the Dictionary for us
			for (int i = 0; i < stats.Item1.Length; i++)
			{
				CardPlayerStat stat = null;
				if ((CardPlayerStatDataType)stats.Item2[i] == CardPlayerStatDataType.Int)
				{
					stat = new CardPlayerStat<int>(int.Parse(stats.Item3[i]),CardPlayerStatDataType.Int);
				}
				else if ((CardPlayerStatDataType)stats.Item2[i] == CardPlayerStatDataType.Float)
				{
					stat = new CardPlayerStat<float>(float.Parse(stats.Item3[i]),CardPlayerStatDataType.Float);
				}
				else if ((CardPlayerStatDataType) stats.Item2[i] == CardPlayerStatDataType.String)
				{
					stat = new CardPlayerStat<string>(stats.Item3[i], CardPlayerStatDataType.String);
				}

				if (stat != null)
				{
					Debug.Log("Adding Stats From network: " + (CardPlayerStatType) stats.Item1[i] + " -> " +
					          stat.GetValue());
					e.Stats.Add((CardPlayerStatType)stats.Item1[i], stat);
				}
			}

			return e;
		}

	}
}