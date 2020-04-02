using UnityEngine;

public class RotateComponent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
	    float amount = 40f*Time.deltaTime;


		transform.Rotate(Vector3.forward, amount);
    }
}
