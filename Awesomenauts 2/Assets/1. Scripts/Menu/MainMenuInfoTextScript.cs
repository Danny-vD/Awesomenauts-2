using UnityEngine;
using UnityEngine.UI;

namespace AwsomenautsCardGame.Menu {
	[RequireComponent(typeof(Text))]
	public class MainMenuInfoTextScript : MonoBehaviour
	{
		private static MainMenuInfoTextScript instance;
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

		private void Awake()
		{
			instance = this;
		}

		private Text t;

		// Start is called before the first frame update
		private void Start()
		{
			t = GetComponent<Text>();
			if (text != null)
			{
				t.text = text;
			}
			text = null;
		}

		// Update is called once per frame
		private void Update()
		{

		}
	}
}
