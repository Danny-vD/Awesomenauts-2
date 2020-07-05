using UnityEngine;
using UnityEngine.UI;

public class RotateComponent : MonoBehaviour
{
	public float RotationSpeed = 40f;
	public float FadeSpeed = 10;
	private float currentFade;
	private Image img;

	private void Start()
	{
		img = GetComponent<Image>();
	}

	// Update is called once per frame
	private void Update()
	{
		float amount = RotationSpeed * Time.deltaTime;
		
		transform.Rotate(Vector3.forward, amount);

		float rot = FadeSpeed * Time.deltaTime;

		currentFade += rot;
		
		img.fillAmount = Mathf.PingPong(currentFade, 1f);

	}
}