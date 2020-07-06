using UnityEngine;

namespace AwsomenautsCardGame.Utility {
	public class DebugDisableScript : MonoBehaviour
	{
		// Start is called before the first frame update
		private void Start()
		{
			if(!Debug.isDebugBuild)gameObject.SetActive(false);
		}

		// Update is called once per frame
		private void Update()
		{
        
		}
	}
}
