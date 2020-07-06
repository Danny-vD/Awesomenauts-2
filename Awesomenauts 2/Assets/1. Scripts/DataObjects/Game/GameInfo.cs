using System;

namespace AwsomenautsCardGame.DataObjects.Game {
	[Serializable]
	public class GameInfo
	{
		public HeadlessServerInfo HeadlessInfo;
		public GameNetworkInfo Network;
		public DebugInfo DebugInfo;
		public int TargetFPS;
		public bool Mute;
		public string StartupAction;

		public GameInfo()
		{
			HeadlessInfo = new HeadlessServerInfo();
			Network = new GameNetworkInfo();
			DebugInfo = new DebugInfo();
			TargetFPS = 60;
		}
	}
}