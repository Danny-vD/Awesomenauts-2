using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Networking;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExceptionViewUI : MonoBehaviour
{

	public Text ExceptionType;
	public Text Title;
	public Text ExceptionMessage;
	public Text StackTrace;
	private Exception ex;
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void BackToMenu()
	{
		SceneManager.LoadScene("MenuScene");
	}

	public void Throw()
	{
		if (ex != null)
		{
			Debug.LogException(ex);
		}
	}


	public void SetException(Exception ex, string titleText = "Error:")
	{
		this.ex = ex;
		gameObject.SetActive(true);
		Title.text = titleText;

		string key = ex.GetType().Name;
		if (ex.GetType() == typeof(Exception))
		{
			key = ex.Message;
		}

		ExceptionType.text = ex.GetType().Name;
		ExceptionMessage.text = ex.Message;
		StackTrace.text = ex.StackTrace;
		if (!Directory.Exists(CardNetworkManager.ErrorPath))
			Directory.CreateDirectory(CardNetworkManager.ErrorPath);
		Stream s = File.Create(CardNetworkManager.GetErrorFile(key));
		TextWriter tw = new StreamWriter(s);
		tw.WriteLine(titleText);
		tw.WriteLine("Exception Type: " + ExceptionType.text + "\n");
		tw.WriteLine("Exception Message: " + ExceptionType.text + "\n");
		tw.WriteLine("StackTrace: \n" + StackTrace.text);
		tw.Dispose();

	}
}
