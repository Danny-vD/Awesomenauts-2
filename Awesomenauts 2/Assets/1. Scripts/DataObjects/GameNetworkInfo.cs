using System;

namespace Networking {
	[Serializable]
	public class GameNetworkInfo
	{
		public EndPointInfo DefaultAddress = new EndPointInfo("localhost", 7778);
		public bool FindMatchOnly;

		public GameNetworkInfo(EndPointInfo defaultAddress = null, bool findMatchOnly = false)
		{
			if (defaultAddress != null)
				DefaultAddress = defaultAddress;
			FindMatchOnly = findMatchOnly;
		}

		public GameNetworkInfo(string ip, int port = -1, bool findMatchOnly = false) : this(new EndPointInfo(ip, port), findMatchOnly)
		{

		}
	}
}