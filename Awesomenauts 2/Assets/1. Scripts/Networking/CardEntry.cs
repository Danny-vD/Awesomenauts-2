using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Byt3.Serialization;
using Byt3.Serialization.Serializers;
using Byt3.Serialization.Serializers.Base;
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

		private class EntityStatisticsSerializer : ASerializer<EntityStatistics>
		{
			private EntityStatSerializer statSerializer = new EntityStatSerializer();
			public override EntityStatistics DeserializePacket(PrimitiveValueWrapper s)
			{
				int len = s.ReadInt();

				EntityStatistics es = new EntityStatistics();
				es.InitializeStatDictionary();

				for (int i = 0; i < len; i++)
				{
					MemoryStream ms = new MemoryStream(s.ReadBytes());
					ms.Position = 0;
					NetworkEntityStat stat = Byt3Serializer.ReadPacket<NetworkEntityStat>(ms);
					es.SetValue(stat.StatType, stat.Value);
				}

				return es;
			}

			public override void SerializePacket(PrimitiveValueWrapper s, EntityStatistics obj)
			{
				s.Write(obj.Stats.Count);
				foreach (KeyValuePair<CardPlayerStatType, CardPlayerStat> cardPlayerStat in obj.Stats)
				{
					NetworkEntityStat stat = new NetworkEntityStat()
					{
						StatType = cardPlayerStat.Key,
						Value = cardPlayerStat.Value.GetValue(),
						ValueType = cardPlayerStat.Value.GetValue().GetType()
					};
					MemoryStream ms = new MemoryStream();
					Byt3Serializer.WritePacket(ms, stat);
					byte[] buf = new byte[ms.Length];
					ms.Position = 0;
					ms.Read(buf, 0, buf.Length);
					s.Write(buf);
				}
			}
		}

		private class NetworkEntityStat
		{
			public CardPlayerStatType StatType;
			public Type ValueType;
			public object Value;
		}

		private class EntityStatSerializer : ASerializer<NetworkEntityStat>
		{
			public override NetworkEntityStat DeserializePacket(PrimitiveValueWrapper s)
			{
				NetworkEntityStat stat = new NetworkEntityStat();
				stat.ValueType = Byt3Serializer.GetTypeByKey(s.ReadString());

				MemoryStream ms = new MemoryStream(s.ReadBytes());
				ms.Position = 0;

				stat.Value = Byt3Serializer.ReadPacket(ms);
				stat.StatType = (CardPlayerStatType)s.ReadInt();

				return stat;
			}

			public override void SerializePacket(PrimitiveValueWrapper s, NetworkEntityStat obj)
			{
				s.Write(obj.ValueType.AssemblyQualifiedName);

				MemoryStream ms = new MemoryStream();
				Byt3Serializer.WritePacket(ms, obj.Value);
				byte[] buf = new byte[ms.Length];
				ms.Position = 0;
				ms.Read(buf, 0, buf.Length);
				s.Write(buf);
				s.Write((int)obj.StatType);
			}
		}

		private class StringSerializer : ASerializer<string>
		{
			public override string DeserializePacket(PrimitiveValueWrapper s)
			{
				return s.ReadString();
			}

			public override void SerializePacket(PrimitiveValueWrapper s, string obj)
			{
				s.Write(obj);
			}
		}

		private class StructSerializer<T> : ASerializer<T>
		where T : struct
		{
			public override void SerializePacket(PrimitiveValueWrapper s, T obj)
			{
				s.WriteSimpleStruct(obj);
			}

			public override T DeserializePacket(PrimitiveValueWrapper s)
			{
				return s.ReadSimpleStruct<T>();
			}
		}

		private static bool init;
		private static void InitializeSerializer()
		{
			if (init) return;
			init = true;
			Byt3Serializer.AddSerializer<EntityStatistics>(new EntityStatisticsSerializer());
			Byt3Serializer.AddSerializer<NetworkEntityStat>(new EntityStatSerializer());
			Byt3Serializer.AddSerializer<string>(new StringSerializer());
			Byt3Serializer.AddSerializer<int>(new StructSerializer<int>());
			Byt3Serializer.AddSerializer<float>(new StructSerializer<float>());
			Byt3Serializer.AddSerializer<bool>(new StructSerializer<bool>());

		}

		//Stats/Designs/etc

		public byte[] StatisticsToNetworkableArray()
		{
			InitializeSerializer();
			MemoryStream ms = new MemoryStream();
			Byt3Serializer.WritePacket(ms, Statistics);
			return ms.GetBuffer().Take((int)ms.Position).ToArray();
			//Tuple<int[], int[], string[]> ret =
			//	new Tuple<int[], int[], string[]>(
			//		new int[Statistics.Stats.Count],
			//		new int[Statistics.Stats.Count],
			//		new string[Statistics.Stats.Count]);
			//int i = 0;
			//foreach (KeyValuePair<CardPlayerStatType, CardPlayerStat> statisticsStat in Statistics.Stats)
			//{
			//	ret.Item1[i] = (int)statisticsStat.Key;
			//	ret.Item2[i] = (int)statisticsStat.Value.Type;
			//	object o = statisticsStat.Value.GetValue();
			//	ret.Item3[i] = o.ToString();
			//	i++;
			//}

			//return ret;
		}
		//int[] Stat Type Enum
		//int[] Stat Type
		//float[] Stat Values
		public static EntityStatistics FromNetwork(byte[] buffer)
		{
			InitializeSerializer();
			MemoryStream ms = new MemoryStream(buffer);
			ms.Position = 0;
			return Byt3Serializer.ReadPacket<EntityStatistics>(ms);
			//EntityStatistics e = new EntityStatistics();
			//e.InitializeStatDictionary(); //Creates the Dictionary for us
			//for (int i = 0; i < stats.Item1.Length; i++)
			//{
			//	CardPlayerStat stat = null;
			//	if ((CardPlayerStatDataType)stats.Item2[i] == CardPlayerStatDataType.Int)
			//	{
			//		stat = new CardPlayerStat<int>(int.Parse(stats.Item3[i]), CardPlayerStatDataType.Int);
			//	}
			//	else if ((CardPlayerStatDataType)stats.Item2[i] == CardPlayerStatDataType.Float)
			//	{
			//		stat = new CardPlayerStat<float>(float.Parse(stats.Item3[i]), CardPlayerStatDataType.Float);
			//	}
			//	else if ((CardPlayerStatDataType)stats.Item2[i] == CardPlayerStatDataType.String)
			//	{
			//		stat = new CardPlayerStat<string>(stats.Item3[i], CardPlayerStatDataType.String);
			//	}

			//	if (stat != null)
			//	{
			//		Debug.Log("Adding Stats From network: " + (CardPlayerStatType)stats.Item1[i] + " -> " +
			//				  stat.GetValue());
			//		e.Stats.Add((CardPlayerStatType)stats.Item1[i], stat);
			//	}
			//return e;
		}

	}

}
