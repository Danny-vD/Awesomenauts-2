using AwsomenautsCardGame.Networking;
using UnityEngine;
using UnityEngine.UI;

namespace AwsomenautsCardGame.Utility {
	public class ButtonDisableScript : MonoBehaviour
	{
		public Button Button;

		private void Start()
		{
			if (CardNetworkManager.Instance == null) Button?.onClick.RemoveAllListeners();
		}
	}
}