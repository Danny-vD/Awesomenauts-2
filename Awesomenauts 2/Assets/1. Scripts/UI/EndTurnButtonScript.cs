using AwsomenautsCardGame.Maps;
using UnityEngine;
using UnityEngine.UI;

namespace AwsomenautsCardGame.UI {
	public class EndTurnButtonScript : MonoBehaviour
	{
		private bool init = false;

		public Button Button;
		// Start is called before the first frame update
		private void Start()
		{
		}

		// Update is called once per frame
		private void Update()
		{
			if (!init && BoardLogic.Logic != null)
			{
				init = true;
				BoardLogic.Logic.OnEndTurn += OnEndTurn;
				BoardLogic.Logic.OnStartTurn += OnStartTurn;
			}
		}

		private void OnStartTurn()
		{
			Button.interactable = true;
		}

		private void OnEndTurn()
		{
			Button.interactable = false;
		}
	}
}
