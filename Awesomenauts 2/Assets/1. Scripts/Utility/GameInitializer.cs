using System;
using System.Collections.Generic;
using Networking;
using CommandRunner;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility.Commands;

public class GameInitializer : NetworkBehaviour
{
	public static GameInitializer Instance;
	public static MasterServerComponent Master => Instance.MasterServerComp;
	public static GameInfo Data => Instance == null ? null : Instance.GameData;

	[TextArea(5, 15)]
	public string StartupArguments;
	public bool ProcessCommandLineArguments;

	public GameInfo GameData;
	private MasterServerComponent MasterServerComp;

	public List<string> AllPaths;

	void Awake()
	{
		DontDestroyOnLoad(gameObject);
		//GameData = new CardNetworkManager.GameInfo(); //Editor does this for us
		MasterServerComp = GetComponent<MasterServerComponent>();
		Instance = this;
		ProcessArgs();
	}

	// Start is called before the first frame update
	void Start()
	{
		SceneManager.LoadScene("MenuScene");
	}


	private void ProcessArgs()
	{

		ReflectionDataCommand refCmd = new ReflectionDataCommand(
			ReflectionDataCommand.Create(
				ReflectionDataCommand.Create("Game", Data),
				ReflectionDataCommand.Create("Master", Master.Info)));

		AllPaths = refCmd.AllPaths;

		Runner.RemoveAllCommands();
		Runner.AddCommand(refCmd);

		if (!Application.isEditor)
		{
			Runner.RunCommands(Environment.GetCommandLineArgs());
		}
		else if(StartupArguments != "")
		{
			string[] args = StartupArguments.Split(' ', '\n');
			Runner.RunCommands(args);
		}
		Runner.RemoveAllCommands();
	}

}
