using System;
using System.Diagnostics;
using UnityEditor;
using System.IO;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

[CreateAssetMenu(menuName = "Scriptable Objects/Multi Build Options")]
public class AutomaticBuildScript : ScriptableObject
{

	public BuildOpts[] Options;
	private static readonly string ToolFolder = Path.GetFullPath(@"./DeployUtilities/");

	private static string TempDir =
		Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(Path.GetRandomFileName()));

	public static void Clean(BuildPlayerOptions opt)
	{
		bool isDir = Directory.Exists(opt.locationPathName);
		string path = isDir ? opt.locationPathName : Path.GetDirectoryName(opt.locationPathName);
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
		ZipFile.CreateFromDirectory(dir, filename);

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
		for (int i = 0; i < Options.Length; i++)
		{
			Deploy(Options[i].ToBuildOptions());
		}
	}

	public void Build()
	{
		for (int i = 0; i < Options.Length; i++)
		{
			Build(Options[i].ToBuildOptions());
		}
	}

	public void Clean()
	{
		for (int i = 0; i < Options.Length; i++)
		{
			Clean(Options[i].ToBuildOptions());
		}
	}
}