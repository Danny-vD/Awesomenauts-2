using System.Diagnostics;
using VDFramework.VDUnityFramework.BaseClasses;

namespace AwsomenautsCardGame.Utility
{
	public class ProcessStarter : BetterMonoBehaviour
	{
		public string ProcessPath = "steam://rungameid/204300";
		
		public static void RunProcess(string path)
		{
			Process.Start(path);
		}

		public void RunProcess()
		{
			RunProcess(ProcessPath);
		}
	}
}