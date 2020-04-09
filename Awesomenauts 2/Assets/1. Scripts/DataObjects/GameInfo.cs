using System;

namespace DataObjects {
	[Serializable]
	public class GameInfo
	{
		public HeadlessServerInfo HeadlessInfo;
		public GameNetworkInfo Network;
		public DebugInfo DebugInfo;
		public int TargetFPS;

		public GameInfo()
		{
			HeadlessInfo = new HeadlessServerInfo();
			Network = new GameNetworkInfo();
			DebugInfo = new DebugInfo();
			TargetFPS = 60;
		}
	}
}