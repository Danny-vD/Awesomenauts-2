using System.Reflection;
using System.Threading;
using Networking;
using UnityEngine;
using UnityEngine.UI;

public class UINetworkHelper : MonoBehaviour
{
	public struct FieldInformation
	{
		public string path;
		public FieldInfo info;
		public object reference;
	}

	public InputField[] fields;

	CancellationTokenSource ts = new CancellationTokenSource();

	public GameObject MainMenu;
	public GameObject LoadingScreen;
	public Text QueueStatus;
	private string StatusText;
	private float TimeStamp;
	private bool DisplayTimestamp;
	
	private EndPointInfo ServerInstance;

	private string TimeStampUI =>
		DisplayTimestamp ? $"({Mathf.RoundToInt(Time.realtimeSinceStartup - TimeStamp).ToString()})" : "";

	public Button buttonFindMatch;
	private bool UpdateQueueStatus;

	void Start()
	{
		foreach (InputField inputField in fields)
		{
			inputField.SetTextWithoutNotify(GameInitializer.Data.Network.DefaultAddress.IP);
		}

		GameInitializer.Master.onMasterServerStartQueue += OnMasterServerStartQueue;
		GameInitializer.Master.onMasterServerFoundMatch += OnMasterServerFoundMatch;
		GameInitializer.Master.onMasterServerConnectToInstance += OnMasterServerConnectToInstance;
		GameInitializer.Master.onMasterServerReset += OnMasterServerReset;

		if (GameInitializer.Data.Network.FindMatchOnly) buttonFindMatch.onClick.Invoke();
	}

	private void OnDestroy()
	{
		ts.Cancel();
		GameInitializer.Master.onMasterServerStartQueue -= OnMasterServerStartQueue;
		GameInitializer.Master.onMasterServerFoundMatch -= OnMasterServerFoundMatch;
		GameInitializer.Master.onMasterServerConnectToInstance -= OnMasterServerConnectToInstance;
		GameInitializer.Master.onMasterServerReset -= OnMasterServerReset;

	}

	private void OnMasterServerReset()
	{
		DisplayTimestamp = false;
		UpdateQueueStatus = false;
		MainMenu?.SetActive(true);
		LoadingScreen?.SetActive(false);
		SetText("Connection Failed.");
	}

	private void OnMasterServerConnectToInstance(int tries)
	{
		DisplayTimestamp = true;
		UpdateQueueStatus = true;
		TimeStamp = Time.realtimeSinceStartup;
		SetText($"Connecting to Game Server {ServerInstance} try {tries}.");
	}

	private void OnMasterServerFoundMatch(EndPointInfo serverinstance)
	{
		ServerInstance = serverinstance;
		TimeStamp = Time.realtimeSinceStartup;
		DisplayTimestamp = false;
		UpdateQueueStatus = true;
		SetText("Found Match on Instance: " + serverinstance);
	}

	private void OnMasterServerStartQueue()
	{
		TimeStamp = Time.realtimeSinceStartup;
		DisplayTimestamp = true;
		UpdateQueueStatus = true;
		SetText("In Queue..");
	}



	//Passthrough for the UI. Since the Network Manager is not loaded in the same scene we need to pass it through something static for the UI to work.

	private void Update()
	{
		if (UpdateQueueStatus)
		{
			UpdateQueueText(StatusText);
		}
	}

	private void SetText(string text)
	{
		StatusText = text;
	}

	private void UpdateQueueText(string text)
	{
		QueueStatus.text =
			$"{text}{TimeStampUI}";
	}

	#region Button Functions

	public void StartClient()
	{
		CardNetworkManager.Instance.ApplyEndPoint();
		CardNetworkManager.Instance.StartClient();
	}

	public void StartHost()
	{
		CardNetworkManager.Instance.ApplyEndPoint();
		CardNetworkManager.Instance.StartHost();
	}


	public void StartServer()
	{
		CardNetworkManager.Instance.ApplyEndPoint();
		CardNetworkManager.Instance.StartServer();
	}
	public void SetCardsInDeck(int id)
	{
		CardNetworkManager.Instance.SetCardsInDeck(id);
	}

	public void IPChanged(string text)
	{
		if (!string.IsNullOrEmpty(text))
		{
			EndPointInfo ep = CardNetworkManager.Instance.CurrentEndPoint;
			ep.IP = text;
			CardNetworkManager.Instance.CurrentEndPoint = ep;
		}
	}

	public void FindGame()
	{
		UpdateQueueStatus = true;
		DisplayTimestamp = true;
		SetText("Connecting to Master Server..");
		GameInitializer.Master.ConnectToServer();
	}
	#endregion


}
