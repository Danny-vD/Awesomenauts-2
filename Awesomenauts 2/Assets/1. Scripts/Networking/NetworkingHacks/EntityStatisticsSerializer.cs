using System.Collections.Generic;
using System.IO;
using AwsomenautsCardGame.Player;
using AwsomenautsCardGame.UI;
using Byt3.Serialization;
using Byt3.Serialization.Serializers;
using Byt3.Utilities.Exceptions;
using MasterServer.Common;

namespace AwsomenautsCardGame.Networking.NetworkingHacks
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

				bool ret = SerializerSingleton.Serializer.TryReadPacket(ms, out NetworkEntityStat stat);
				if (!ret)
				{
					ExceptionViewUI.Instance.SetException(new Byt3Exception("Entity Statistics Read packet Exception"), "Deserialization Exception:");
				}
				else
				{
					es.SetValue(stat.StatType, stat.Value);
				}

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
				if (!SerializerSingleton.Serializer.TryWritePacket(ms, stat))
				{
					ExceptionViewUI.Instance.SetException(new Byt3Exception("Entity Statistics Serializer Write Error"), "Serialization Exception:");
				}
				else
				{
					byte[] buf = new byte[ms.Length];
					ms.Position = 0;
					ms.Read(buf, 0, buf.Length);
					s.Write(buf);
				}

			}
		}
	}
}