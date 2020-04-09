using VDFramework.VDUnityFramework.BaseClasses;

namespace UI
{
	public class ButtonFunctionality : BetterMonoBehaviour
	{
		public void QuitApplication()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.ExitPlaymode();
#else
			UnityEngine.Application.Quit();
#endif
		}
	}
}