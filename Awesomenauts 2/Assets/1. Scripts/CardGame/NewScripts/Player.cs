using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Player : NetworkBehaviour
{

	public bool DisableInteractions;
	public int AssociatedClient;

	// Start is called before the first frame update
    void Start()
    {
	    CardNetworkManager.Instance.RegisterPlayer(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
