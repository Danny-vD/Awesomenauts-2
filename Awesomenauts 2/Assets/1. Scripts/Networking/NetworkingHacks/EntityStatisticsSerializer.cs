using System;
using System.Collections.Generic;
using System.IO;
using Byt3.Serialization;
using Byt3.Serialization.Serializers;
using Networking.Statistics;
using Player;

namespace Networking.NetworkingHacks
{
	public class EntityStatisticsSerializer : ASerializer<EntityStatistics>
	{
		public override EntityStatistics DeserializePacket(PrimitiveValueWrapper s)
		{
			int len = s.ReadInt();

			EntityStatistics es = new EntityStatistics();
			es.InitializeStatDictionary();

			for (int i = 0; i < len; i++)
			{
				MemoryStream ms = new MemoryStream(s.ReadBytes()) { Position = 0 };

				bool ret = Byt3Serializer.TryReadPacket(ms, out NetworkEntityStat stat);
				if (!ret) throw new Exception("Read packet Exception");

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
				if (!Byt3Serializer.TryWritePacket(ms, stat)) throw new Exception("Serializer Write Error");

				byte[] buf = new byte[ms.Length];
				ms.Position = 0;
				ms.Read(buf, 0, buf.Length);
				s.Write(buf);
			}
		}
	}
}