using UnityEngine;

public class DebugDisableScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		if(!Debug.isDebugBuild)gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
