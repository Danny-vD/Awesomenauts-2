using UnityEngine;

namespace UI
{
	public class ExitHelper : MonoBehaviour
	{
		public void Exit()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.ExitPlaymode();
#else
			UnityEngine.Application.Quit();
#endif
		}
	}
}