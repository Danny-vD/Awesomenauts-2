using System;

namespace AwsomenautsCardGame.DataObjects.Game
{
	[Serializable]
	public class DebugInfo
	{
		public bool DebugServerQuit;
		public bool AllowUnrestrictedDecks;
		public bool NoShuffleDecks;
		public int StartSolar = -1;


		public DebugInfo(bool debugServerQuit = false)
		{
			DebugServerQuit = debugServerQuit;
		}
	}
}