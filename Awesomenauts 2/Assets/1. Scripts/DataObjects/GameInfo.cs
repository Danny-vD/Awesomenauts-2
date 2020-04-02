using System;

namespace Networking {
	[Serializable]
	public class GameInfo
	{
		public HeadlessServerInfo HeadlessInfo;
		public GameNetworkInfo Network;
		public DebugInfo DebugInfo;

		public GameInfo()
		{
			HeadlessInfo = new HeadlessServerInfo();
			Network = new GameNetworkInfo();
			DebugInfo = new DebugInfo();
		}
	}
}