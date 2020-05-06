using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace BuildInfo.AutomaticBuildScripts
{
	[CreateAssetMenu(menuName = "Scriptable Objects/Multi Build Options")]
	public class AutomaticBuildScript : ScriptableObject
	{
		public bool ForceReleaseBuild;
		public BuildOpts[] Options;
		private static readonly string ToolFolder = Path.GetFullPath(@"./DeployUtilities/");

		private static string TempDir =
			Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(Path.GetRandomFileName()));

		public static void Clean(BuildPlayerOptions opt)
		{
			bool isDir = Directory.Exists(opt.locationPathName);
			string path = isDir ? opt.locationPathName : Path.GetDirectoryName(opt.locationPathName);

			string filename = Path.Combine(Path.GetDirectoryName(path), Path.GetFileName(path) + ".zip");
			if (File.Exists(filename))
			{
				File.Delete(filename);
			}

			if (Directory.Exists(path))
			{
				Debug.Log("Cleaning Directory: " + Path.GetFullPath(path));
				Directory.Delete(path, true);
			}
		}

		public static void Deploy(BuildPlayerOptions opt)
		{
			bool isDir = Directory.Exists(opt.locationPathName);
			string dir = isDir ? opt.locationPathName : Path.GetDirectoryName(opt.locationPathName);

			string filename = Path.Combine(Path.GetDirectoryName(dir), Path.GetFileName(dir) + ".zip");


			//DIR => Source
			//Filename => Target

			//Uses Assets/zExternalCode/System.IO.Compression/System.IO.Compression.dll
            
			//NOTE: Sorry tim, had to comment it out so that my stuff will compile.
			//powershell .\Zip.ps1 -in MasterServer\bin\Release\netcoreapp2.1\win-x64\publish -out MasterServer_Windows.zip
			CreateZipFromDir(dir, filename);
		}

		private static void CreateZipFromDir(string dir, string output)
		{
			Process p = new Process();
			ProcessStartInfo pi = new ProcessStartInfo("powershell", $"-WindowStyle Hidden .\\Zip.ps1 -in {dir} -out {output}");
			pi.WorkingDirectory = Directory.GetCurrentDirectory();
			pi.RedirectStandardOutput = true;
			pi.RedirectStandardError = true;
			pi.UseShellExecute = false;
			p.StartInfo = pi;
			p.Start();
			p.WaitForExit();
            Debug.Log("ZipLog:\n" +p.StandardOutput.ReadToEnd()+"\nZipError:\n"+ p.StandardError.ReadToEnd());
		}

		public static void Build(BuildPlayerOptions opt)
		{
			string s = "";
			for (int i = 0; i < opt.scenes.Length; i++)
			{
				s += opt.scenes[i] + ";";
			}

			Debug.Log("Building Scenes: " + s);
			bool isDir = Directory.Exists(opt.locationPathName);
			string path = isDir ? opt.locationPathName : Path.GetDirectoryName(opt.locationPathName);
			if (!Directory.Exists(path))
			{
				Debug.Log("Creating Directory: " + Path.GetFullPath(path));
				Directory.CreateDirectory(Path.GetFullPath(path));
			}

			Debug.Log("Building: " + opt.target + "@" + opt.locationPathName);

			BuildReport rep = BuildPipeline.BuildPlayer(opt);
			Debug.Log("Build: " + rep.summary.result);
		}

		public void Deploy()
		{ 
            Task[] tasks = new Task[Options.Length];
			for (int i = 0; i < Options.Length; i++)
			{
				BuildPlayerOptions opts = Options[i].ToBuildOptions(Options[i].options);

				Task t = new Task(() => Deploy(opts));
				t.Start();
				tasks[i] = t;
			}

			Task.WaitAll(tasks);
		}

		public void Build(bool forceReleaseBuild)
		{
			for (int i = 0; i < Options.Length; i++)
			{
				Build(Options[i].ToBuildOptions(CleanDevelopBuildFlag(Options[i].options)));
			}
		}

		private BuildOptions CleanDevelopBuildFlag(BuildOptions options)
		{
			return options & ~BuildOptions.Development;
		}

		public void Clean()
		{
			for (int i = 0; i < Options.Length; i++)
			{
				Clean(Options[i].ToBuildOptions(Options[i].options));
			}
		}
	}
}