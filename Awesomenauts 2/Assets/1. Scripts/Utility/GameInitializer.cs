using System;
using System.Collections.Generic;
using DataObjects;
using Networking;
using Utility.Commands;
using Byt3.CommandRunner;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utility {
	public class GameInitializer : NetworkBehaviour
	{
		public static GameInitializer Instance;
		public static MasterServerComponent Master => Instance.MasterServerComp;
		public static GameInfo Data => Instance == null ? null : Instance.GameData;

		[TextArea(5, 15)]
		public string StartupArguments;
		public bool ProcessCommandLineArguments;

		[Scene]
		public string NextScene;

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
			Application.targetFrameRate = GameData.TargetFPS;
		}

		// Start is called before the first frame update
		void Start()
		{
			SceneManager.LoadScene(NextScene);
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
			else if (StartupArguments != "")
			{
				string[] args = StartupArguments.Split(' ', '\n');
				Runner.RunCommands(args);
			}
			Runner.RemoveAllCommands();
		}

	}
}
