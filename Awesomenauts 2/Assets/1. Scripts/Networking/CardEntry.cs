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

		public Tuple<int[], int[], float[]> StatisticsToNetworkableTypes()
		{

			Tuple<int[], int[], float[]> ret = 
				new Tuple<int[], int[], float[]>(
					new int[Statistics.Stats.Count],
					new int[Statistics.Stats.Count], 
					new float[Statistics.Stats.Count]);
			int i = 0;
			foreach (KeyValuePair<CardPlayerStatType, CardPlayerStat> statisticsStat in Statistics.Stats)
			{
				ret.Item1[i] = (int)statisticsStat.Key;
				ret.Item2[i] = (int)statisticsStat.Value.Type;
				float v = -1;
				object o = statisticsStat.Value.GetValue();
				if (o is int ii) v = ii;
				else if (o is float ff) v = ff;
				ret.Item3[i] = v;
			}

			return ret;
		}
		//int[] Stat Type Enum
		//int[] Stat Type
		//float[] Stat Values
		public static EntityStatistics FromNetwork(Tuple<int[], int[], float[]> stats)
		{
			EntityStatistics e = new EntityStatistics();
			e.InitializeStatDictionary(); //Creates the Dictionary for us
			for (int i = 0; i < stats.Item1.Length; i++)
			{
				CardPlayerStat stat = null;
				if ((CardPlayerStatDataType)stats.Item2[i] == CardPlayerStatDataType.Int)
				{
					stat = new CardPlayerStat<int>((int)stats.Item3[i],CardPlayerStatDataType.Int);
				}
				else if ((CardPlayerStatDataType)stats.Item2[i] == CardPlayerStatDataType.Float)
				{
					stat = new CardPlayerStat<float>(stats.Item3[i],CardPlayerStatDataType.Float);
				}
				if (stat != null)
					e.Stats.Add((CardPlayerStatType)stats.Item1[i], stat);
			}

			return e;
		}

	}
}