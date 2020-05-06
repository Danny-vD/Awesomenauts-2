using System;
using System.IO;
using Byt3.Serialization;
using Byt3.Serialization.Serializers;
using MasterServer.Common;
using Networking.Statistics;
using Player;
using UnityEngine;

namespace Networking.NetworkingHacks {
	public class EntityStatSerializer : ASerializer<NetworkEntityStat>
	{
		public override NetworkEntityStat DeserializePacket(PrimitiveValueWrapper s)
		{
			NetworkEntityStat stat = new NetworkEntityStat
			{
				ValueType = SerializerSingleton.Serializer.GetTypeByKey(s.ReadString())
			};

			MemoryStream ms = new MemoryStream(s.ReadBytes()) { Position = 0 };

			bool ret = SerializerSingleton.Serializer.TryReadPacket(ms, out stat.Value);
			if (!ret) throw new Exception("Read packet Exception");

			stat.StatType = (CardPlayerStatType)s.ReadInt();

			return stat;
		}

		public override void SerializePacket(PrimitiveValueWrapper s, NetworkEntityStat obj)
		{
			s.Write(obj.ValueType.AssemblyQualifiedName);

			MemoryStream ms = new MemoryStream();
			if (SerializerSingleton.Serializer.CanSerialize(typeof(int)))
			{
				Debug.Log("Serializable");
			}
			else
			{
				Debug.Log("WTF");
			}
			if (!SerializerSingleton.Serializer.TryWritePacket(ms, obj.Value))
				throw new Exception("Serializer Write Error");

			byte[] buf = new byte[ms.Length];
			ms.Position = 0;
			ms.Read(buf, 0, buf.Length);
			s.Write(buf);
			s.Write((int)obj.StatType);
		}
	}
}