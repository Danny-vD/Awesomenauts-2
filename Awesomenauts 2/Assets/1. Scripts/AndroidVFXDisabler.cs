using System.Collections.Generic;
using UnityEngine;

public class AndroidVFXDisabler : MonoBehaviour
{

	public List<GameObject> DisableObjects;
    // Start is called before the first frame update
    void Start()
    {
	    if (Application.platform == RuntimePlatform.Android)
	    {
			DisableObjects.ForEach(Destroy);
	    }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
