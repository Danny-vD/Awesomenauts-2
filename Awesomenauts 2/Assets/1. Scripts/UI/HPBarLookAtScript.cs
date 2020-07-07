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
			transform.up = c.transform.up;
			Vector3 forward = c.transform.position - transform.position;
			forward.z = 0;
			transform.forward = forward;
		}
	}
}
