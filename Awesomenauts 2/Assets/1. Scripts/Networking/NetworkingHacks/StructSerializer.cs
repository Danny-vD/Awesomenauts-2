using Byt3.Serialization;
using Byt3.Serialization.Serializers;

namespace AwsomenautsCardGame.Networking.NetworkingHacks {
	public class StructSerializer<T> : ASerializer<T>
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
}