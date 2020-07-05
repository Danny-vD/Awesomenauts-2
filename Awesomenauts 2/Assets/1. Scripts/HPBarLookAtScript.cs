using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.UI;

public class HPBarLookAtScript : MonoBehaviour
{
	private Camera c;
	
	// Start is called before the first frame update
	void Start()
	{
		c = Camera.main;
	}

	// Update is called once per frame
	void Update()
	{
		transform.LookAt(c.transform);
	}
}