using System;

namespace DataObjects {
	[Serializable]
	public class DebugInfo
	{
		public bool DebugServerQuit;
		public bool AllowUnrestrictedDecks;
		public bool NoShuffleDecks;


		public DebugInfo(bool debugServerQuit = false)
		{
			DebugServerQuit = debugServerQuit;
		}
	}
}