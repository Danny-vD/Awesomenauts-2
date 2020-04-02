using System;

namespace Networking {
	[Serializable]
	public class MasterServerInfo
	{

		public EndPointInfo Address=new EndPointInfo("213.109.162.193", 19999);
		public int ConnectInstanceTimeout= 5000;
		public int ConnectMasterTimeout= 5000;
		public int ConnectInstanceTries=5;
		public int ConnectMasterTries=3;

		public MasterServerInfo(EndPointInfo address = null, int connectInstanceTimeout = 5000, int connectMasterTimeout = 5000, int connectInstanceTries = 5, int connectMasterTries = 3)
		{
			if (address != null)
				Address = address;
			ConnectInstanceTimeout = connectInstanceTimeout;
			ConnectMasterTimeout = connectMasterTimeout;
			ConnectInstanceTries = connectInstanceTries;
			ConnectMasterTries = connectMasterTries;
		}

		public MasterServerInfo(string ip, int port = -1, int connectInstanceTimeout = 5000, int connectMasterTimeout = 5000, int connectInstanceTries = 5, int connectMasterTries = 3) : this(new EndPointInfo(ip, port), connectInstanceTimeout, connectMasterTimeout, connectInstanceTries, connectMasterTries)
		{

		}
	}
}