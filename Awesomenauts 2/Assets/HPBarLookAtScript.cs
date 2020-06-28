using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.UI;

public class HPBarLookAtScript : MonoBehaviour
{
	private Camera c;

	public Slider s;
    // Start is called before the first frame update
    void Start()
    {
        c = Camera.main;
        GetComponentInParent<Card>().Statistics.Register(CardPlayerStatType.HP, OnHPChanged, true);
    }

    private void OnHPChanged(object newvalue)
    {
	    s.value = (float)(int) newvalue / 10;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(c.transform);
    }
}
