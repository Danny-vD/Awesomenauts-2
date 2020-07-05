using UnityEngine;

public class RotateComponent : MonoBehaviour
{
	// Update is called once per frame
	private void Update()
	{
		float amount = 40f * Time.deltaTime;

		transform.Rotate(Vector3.forward, amount);
	}
}