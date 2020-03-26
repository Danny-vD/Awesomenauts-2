using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Card : NetworkBehaviour
{
	public MeshRenderer CoverUpRenderer;
    // Start is called before the first frame update
    void Start()
    {
	    if (isClient && !hasAuthority)
	    {
		    Debug.Log("Enable Cover Up Renderer");
		    CoverUpRenderer.enabled = true;
	    }
    }
}
