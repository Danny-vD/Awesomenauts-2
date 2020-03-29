using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINetworkHelper : MonoBehaviour
{
	public InputField[] fields;

	void Start()
	{
		ip = CardNetworkManager.Instance.TransportInfoData.DefaultIP;

		foreach (InputField inputField in fields)
		{
			inputField.SetTextWithoutNotify(ip);
		}
	}

	private string ip;
	public void IPChanged(string text)
	{
		if (!string.IsNullOrEmpty(text))
			ip = text;
	}

	public void StartClient()
	{
		CardNetworkManager.Instance.networkAddress = ip;
		CardNetworkManager.Instance.StartClient();
	}

	public void StartHost()
	{
		CardNetworkManager.Instance.networkAddress = ip;
		CardNetworkManager.Instance.StartHost();
	}


	public void StartServer()
	{
		CardNetworkManager.Instance.networkAddress = ip;
		CardNetworkManager.Instance.StartServer();
	}
}
