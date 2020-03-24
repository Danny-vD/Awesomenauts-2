using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSocket : MonoBehaviour
{
	private bool initiated;
	private float origY;
	private float yScale;
	private float yOffset;
	private float yCardOffset;
	private float timeScale;
	private float timeOffset;

	private Transform dockedTransform;
	// Start is called before the first frame update
	void Start()
	{
		origY = transform.position.y;
		initiated = true;
	}

	public void SetOffsetAndSpeed(float offset, float speed, float yScale, float yOffset, float yCardOffset)
	{
		timeScale = speed;
		timeOffset = offset;
		this.yScale = yScale;
		this.yOffset = yOffset;
		this.yCardOffset = yCardOffset;
	}

	public void Animate(bool animationState)
	{
		enabled = animationState;
		if (initiated && !animationState) SetDisableState();
	}


	public void DockTransform(Transform dock)
	{
		dockedTransform = dock;
	}

	public void SetDisableState()
	{
		Vector3 pos = transform.position;
		pos.y = origY;
		if (dockedTransform != null)
		{
			dockedTransform.position = pos + Vector3.up * yCardOffset;
		}
		transform.position = pos;
	}

	// Update is called once per frame
	void Update()
	{
		Vector3 pos = transform.position;
		pos.y = origY + yOffset + Mathf.Sin(Time.realtimeSinceStartup * timeScale + timeOffset) * yScale;
		if (dockedTransform != null)
		{
			dockedTransform.position = pos + Vector3.up * yCardOffset;
		}
		transform.position = pos;
	}
}
