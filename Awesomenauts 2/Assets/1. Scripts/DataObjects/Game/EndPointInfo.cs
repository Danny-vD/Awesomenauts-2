using System;

namespace AwsomenautsCardGame.DataObjects.Game {
	[Serializable]
	public class EndPointInfo
	{
		public string IP;
		public int Port;

		public EndPointInfo(string ip = "localhost", int port = -1)
		{
			IP = ip;
			Port = port;
		}

		public override string ToString()
		{
			return $"{IP}:{Port}";
		}
	}
}