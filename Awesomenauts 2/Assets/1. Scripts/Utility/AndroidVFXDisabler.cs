using System.Collections.Generic;
using UnityEngine;

namespace AwsomenautsCardGame.Utility {
	public class AndroidVFXDisabler : MonoBehaviour
	{

		public List<GameObject> DisableObjects;
		// Start is called before the first frame update
		private void Start()
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				DisableObjects.ForEach(Destroy);
			}
		}

		// Update is called once per frame
		private void Update()
		{
        
		}
	}
}
