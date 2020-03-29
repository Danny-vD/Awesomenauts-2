using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
[CreateAssetMenu(menuName = "Scriptable Objects/Single Build Options")]
public class BuildOpts : ScriptableObject
{
    
	/// <summary>
	///   <para>The Scenes to be included in the build. If empty, the currently open Scene will be built. Paths are relative to the project folder (AssetsMyLevelsMyScene.unity).</para>
	/// </summary>
	public string[] scenes
	{
		get
		{
			List<string> r = new List<string>();
			foreach (var scene in EditorBuildSettings.scenes)
			{
				if (scene.enabled)
					r.Add(scene.path);
			}
            

			return r.ToArray();
		}
	}

	/// <summary>
	///   <para>The path where the application will be built.</para>
	/// </summary>
	public string locationPathName;

	/// <summary>
	///   <para>The BuildTargetGroup to build.</para>
	/// </summary>
	public BuildTargetGroup targetGroup => BuildPipeline.GetBuildTargetGroup(target);

	/// <summary>
	///   <para>The BuildTarget to build.</para>
	/// </summary>
	public BuildTarget target;

	/// <summary>
	///   <para>Additional BuildOptions, like whether to run the built player.</para>
	/// </summary>
	public BuildOptions options;


	public BuildPlayerOptions ToBuildOptions()
	{
		return new BuildPlayerOptions()
		{
			locationPathName = locationPathName,
			options = options,
			scenes = scenes,
			target = target,
			targetGroup = targetGroup
		};
	}
}