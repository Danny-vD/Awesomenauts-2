using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ShowDataPathScript : MonoBehaviour
{
	private static ShowDataPathScript instance;
	private static string text;
	public static void Write(string line)
	{
		if (instance != null)
		{
			instance.t.text = line;
		}
		else
		{
			text = line;
		}
	}

	void Awake()
	{
		instance = this;
	}

	private Text t;

	// Start is called before the first frame update
	void Start()
	{
		t = GetComponent<Text>();
		if (text != null)
			t.text = text;
		text = null;
	}

	// Update is called once per frame
	void Update()
	{

	}
}
