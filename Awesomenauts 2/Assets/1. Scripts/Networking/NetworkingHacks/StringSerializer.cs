using Byt3.Serialization;
using Byt3.Serialization.Serializers;

namespace AwsomenautsCardGame.Networking.NetworkingHacks {
	public class StringSerializer : ASerializer<string>
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
}