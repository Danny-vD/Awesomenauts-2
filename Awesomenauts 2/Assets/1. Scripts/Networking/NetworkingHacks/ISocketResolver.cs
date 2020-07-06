using AwsomenautsCardGame.Maps;

namespace AwsomenautsCardGame.Networking.NetworkingHacks {
	public interface ISocketResolver
	{
		CardSocket GetSocket();
	}
}