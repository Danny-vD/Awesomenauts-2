using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameLimiter : MonoBehaviour
{
	public int FrameLimit = 15;
    // Start is called before the first frame update
    void Start()
	{
#if UNITY_EDITOR
		//Keep Dannys GPU from exploding
		Application.targetFrameRate = FrameLimit;
#else
		//Lock to Screen FPS
		QualitySettings.vSyncCount = 1;
#endif
	}



	// Update is called once per frame
	void Update()
    {
        
    }
}
