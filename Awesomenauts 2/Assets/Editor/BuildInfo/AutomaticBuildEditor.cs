using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.IO;
using UnityEditor;

[CustomEditor(typeof(AutomaticBuildScript))]
public class AutomaticBuildEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		AutomaticBuildScript myScript = (AutomaticBuildScript)target;
		if (GUILayout.Button("Build All"))
		{
			myScript.Build();
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
			myScript.Build();
			myScript.Deploy();
		}
		if (File.Exists("../../AwsomenautsDeploy/upload.bat") && GUILayout.Button("Upload All"))
		{
			myScript.Clean();
			myScript.Build();
			myScript.Deploy();
			Process.Start("cmd.exe", "/C ..\\..\\AwsomenautsDeploy\\upload.bat").WaitForExit();
		}
	}
}

[CustomEditor(typeof(BuildOpts))]
public class BuildOptionEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();


		BuildOpts myScript = (BuildOpts)target;
		if (GUILayout.Button("Build"))
		{
			AutomaticBuildScript.Build(myScript.ToBuildOptions());
		}
		if (GUILayout.Button("Clean"))
		{
			AutomaticBuildScript.Clean(myScript.ToBuildOptions());
		}
		if (GUILayout.Button("Deploy"))
		{
			AutomaticBuildScript.Deploy(myScript.ToBuildOptions());
		}
		if (GUILayout.Button("Rebuild"))
		{
			AutomaticBuildScript.Clean(myScript.ToBuildOptions());
			AutomaticBuildScript.Build(myScript.ToBuildOptions());
			AutomaticBuildScript.Deploy(myScript.ToBuildOptions());
		}
	}
}

