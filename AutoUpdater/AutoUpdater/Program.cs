using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoUpdater
{
	internal static class Program
	{
		public const string CONFIG_PATH = "./AutoUpdater.conf";
		public const string SERVER_CONFIG_PATH = "http://213.109.162.193/apps/games/AwsomenautsCardGame/Archives/AutoUpdater.conf";
		public const string SERVER_PAYLOAD_PATH = "http://213.109.162.193/apps/games/AwsomenautsCardGame/Archives/WindowsStandalone.zip";

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{
			if (args.Length == 1)
			{
				File.WriteAllText(CONFIG_PATH, GetHash(args[0]));
				return;
			}

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Form1());
		}
		public static string GetHash(string File)
		{
			StringBuilder sb = new StringBuilder();
			foreach (byte b in ComputeHash(File))
				sb.Append(b.ToString("X2"));

			return sb.ToString();
		}

		public static byte[] ComputeHash(string file)
		{
			using (HashAlgorithm algorithm = SHA256.Create())
			{
				Stream s = File.OpenRead(file);
				byte[] ret = algorithm.ComputeHash(s);
				s.Close();
				return ret;
			}
		}
	}
}
