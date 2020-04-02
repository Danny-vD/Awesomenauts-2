using System;

namespace Networking {
	[Serializable]
	public class DebugInfo
	{
		public bool DebugServerQuit;

		public DebugInfo(bool debugServerQuit = false)
		{
			DebugServerQuit = debugServerQuit;
		}
	}
}