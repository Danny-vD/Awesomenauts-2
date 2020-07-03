using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExceptionViewUI : MonoBehaviour
{

	public Text Title;
	public Text Message;

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

    public void SetMessage(string title, string text)
    {
		gameObject.SetActive(true);
		Title.text = title;
		Message.text = text;
    }
}
