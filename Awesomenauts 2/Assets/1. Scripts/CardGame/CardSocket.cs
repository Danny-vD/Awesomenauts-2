using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSocket : MonoBehaviour
{
	private float origY;
	private float yScale;
	private float timeScale;
	private float timeOffset;
	// Start is called before the first frame update
	void Start()
	{
		origY = transform.position.y;
	}

	public void SetOffsetAndSpeed(float offset, float speed, float yScale)
	{
		timeScale = speed;
		timeOffset = offset;
		this.yScale = yScale;
	}

	public void Animate(bool animationState)
	{
		enabled = animationState;
	}

	void OnDisable()
	{
		Vector3 pos = transform.position;
		pos.y = origY;
		transform.position = pos;
	}

	// Update is called once per frame
	void Update()
	{
		Vector3 pos = transform.position;
		pos.y = origY + Mathf.Sin(Time.realtimeSinceStartup * timeScale + timeOffset) * yScale;
		transform.position = pos;
	}
}
