using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace BuildInfo.AutomaticBuildScripts
{
	[CustomEditor(typeof(AutomaticBuildScript))]
	public class AutomaticBuildEditor : UnityEditor.Editor
	{
		private bool enable = true;

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			if (!enable) return;
			if (target == null) return;
			AutomaticBuildScript myScript = (AutomaticBuildScript)target;
			//if (GUILayout.Button("Build All"))
			//{
			//	myScript.Build(myScript.ForceReleaseBuild);
			//}

			//if (GUILayout.Button("Clean All"))
			//{
			//	myScript.Clean();
			//}

			//if (GUILayout.Button("Deploy All"))
			//{
			//	myScript.Deploy();
			//}

			//if (GUILayout.Button("Rebuild All"))
			//{
			//	myScript.Clean();
			//	myScript.Build(myScript.ForceReleaseBuild);
			//	myScript.Deploy();
			//}

			if (File.Exists("..\\..\\AwsomenautsDeploy\\Console\\ChangeVersion.bat") && GUILayout.Button("Increase Version"))
			{
				Process p = Process.Start("cmd.exe", "/C call ..\\..\\AwsomenautsDeploy\\Console\\ChangeVersion.bat");
			}

			if (File.Exists("../../AwsomenautsDeploy/upload.bat") && GUILayout.Button("Upload All"))
			{
				enable = false;

				myScript.Clean();
				myScript.Build(true);
				myScript.Deploy();
				Process p1 = Process.Start("cmd.exe", "/C ..\\..\\AwsomenautsDeploy\\upload.bat");
				p1.EnableRaisingEvents = true;
				p1.Exited += (sender1, args1) => OnClose();
			}
		}

		private void OnClose()
		{
			enable = true;
		}
	}

	[CustomEditor(typeof(BuildOpts))]
	public class BuildOptionEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();


			if (target == null) return;
			BuildOpts myScript = (BuildOpts)target;
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