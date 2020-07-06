using UnityEngine;

namespace AwsomenautsCardGame.UI {
	public class HPBarLookAtScript : MonoBehaviour
	{
		private Camera c;
	
		// Start is called before the first frame update
		private void Start()
		{
			c = Camera.main;
		}

		// Update is called once per frame
		private void Update()
		{
			transform.LookAt(c.transform);
		}
	}
}
