using System;

namespace AwsomenautsCardGame.DataObjects
{
	[Serializable]
	public class HeadlessServerInfo
	{
		public int Timeout = 3600 * 1000;
		public int NoPlayerTimeout = 60000;
		public bool CloseOnMatchEnded;
		public float NoPlayerTimeoutSeconds => NoPlayerTimeout / 1000f;
		public float TimeoutSeconds => Timeout / 1000f;

		public HeadlessServerInfo(int timeout = 3600 * 1000, int noPlayerTimeout = 60000)
		{
			Timeout = timeout;
			NoPlayerTimeout = noPlayerTimeout;
		}
	}
}