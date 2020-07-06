using System;
using System.Collections.Generic;
using AwsomenautsCardGame.DataObjects;
using AwsomenautsCardGame.Networking;
using AwsomenautsCardGame.UI;
using AwsomenautsCardGame.Utility.Commands;
using Byt3.CommandRunner;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AwsomenautsCardGame.Utility
{
	public class GameInitializer : NetworkBehaviour
	{
		public static GameInitializer Instance;
		public static MasterServerComponent Master => Instance?.MasterServerComp;
		public static GameInfo Data => Instance == null ? null : Instance.GameData;


		private static Dictionary<string, Action> StartupActions => new Dictionary<string, Action>
		{
			{"host", UINetworkHelper.Instance.StartHost },
			{"join", UINetworkHelper.Instance.StartClient },
			{"queue", UINetworkHelper.Instance.FindGame },
			{"deck", () => SceneManager.LoadScene("DeckBuilder") },
			{"server", UINetworkHelper.Instance.StartServer },

		};


		[TextArea(5, 15)]
		public string StartupArguments;
		public bool ProcessCommandLineArguments;

		[Scene]
		public string NextScene;

		public GameInfo GameData;
		private MasterServerComponent MasterServerComp;

		public int[] DefaultDeck;

		public List<string> AllPaths;

		private void Awake()
		{
			DontDestroyOnLoad(gameObject);
			//GameData = new CardNetworkManager.GameInfo(); //Editor does this for us
			MasterServerComp = GetComponent<MasterServerComponent>();
			Instance = this;
			ProcessArgs();
			Application.targetFrameRate = GameData.TargetFPS;
		}

		// Start is called before the first frame update
		private void Start()
		{
			SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;
			SceneManager.LoadScene(NextScene);
		}

		private void SceneManagerOnSceneLoaded(Scene arg0, LoadSceneMode arg1)
		{
			if (arg0.name == NextScene && GameData.StartupAction != null)
			{
				string[] actions = GameData.StartupAction.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

				foreach (string action in actions)
				{
					if (StartupActions.ContainsKey(action))
					{
						StartupActions[action]?.Invoke();
					}
				}


			}
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

			if (!Application.isEditor && !Debug.isDebugBuild)
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
