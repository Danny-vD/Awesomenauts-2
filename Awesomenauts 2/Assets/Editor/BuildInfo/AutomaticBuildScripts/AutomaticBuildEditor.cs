using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace BuildInfo.AutomaticBuildScripts
{
	[CustomEditor(typeof(AutomaticBuildScript))]
	public class AutomaticBuildEditor : Editor
	{
		private bool enable = true;

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			if (!enable) return;

			AutomaticBuildScript myScript = (AutomaticBuildScript) target;
			if (GUILayout.Button("Build All"))
			{
				myScript.Build(myScript.ForceReleaseBuild);
			}

			if (GUILayout.Button("Clean All"))
			{
				myScript.Clean();
			}

			if (GUILayout.Button("Deploy All"))
			{
				myScript.Deploy();
			}

			if (GUILayout.Button("Rebuild All"))
			{
				myScript.Clean();
				myScript.Build(myScript.ForceReleaseBuild);
				myScript.Deploy();
			}

			if (File.Exists("../../AwsomenautsDeploy/upload.bat") && GUILayout.Button("Upload All"))
			{
				myScript.Clean();
				myScript.Build(true);
				myScript.Deploy();

				enable = false;
				Process p = Process.Start("cmd.exe", "/C ..\\..\\AwsomenautsDeploy\\upload.bat");
				p.EnableRaisingEvents = true;
				p.Exited += (sender, args) => OnClose();
			}
		}

		private void OnClose()
		{
			enable = true;
		}
	}

	[CustomEditor(typeof(BuildOpts))]
	public class BuildOptionEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();


			BuildOpts myScript = (BuildOpts) target;
			if (GUILayout.Button("Build"))
			{
				AutomaticBuildScript.Build(myScript.ToBuildOptions(myScript.options));
			}

			if (GUILayout.Button("Clean"))
			{
				AutomaticBuildScript.Clean(myScript.ToBuildOptions(myScript.options));
			}

			if (GUILayout.Button("Deploy"))
			{
				AutomaticBuildScript.Deploy(myScript.ToBuildOptions(myScript.options));
			}

			if (GUILayout.Button("Rebuild"))
			{
				AutomaticBuildScript.Clean(myScript.ToBuildOptions(myScript.options));
				AutomaticBuildScript.Build(myScript.ToBuildOptions(myScript.options));
				AutomaticBuildScript.Deploy(myScript.ToBuildOptions(myScript.options));
			}
		}
	}
}