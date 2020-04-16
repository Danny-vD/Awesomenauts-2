using Enums.Deckbuilder;
using Events.Deckbuilder;
using VDFramework;
using UnityEngine.SceneManagement;
using VDFramework.EventSystem;

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

		public void LoadScene(string scene)
		{
			SceneManager.LoadScene(scene);
		}

		public void LoadScene(int buildIndex)
		{
			SceneManager.LoadScene(buildIndex);
		}

		public void ShowAllCards()
		{
			EventManager.Instance.RaiseEvent(new FiltersChangedEvent(FilterValues.ShowAll));
		}
		
		public void ShowAllOwned()
		{
			EventManager.Instance.RaiseEvent(new FiltersChangedEvent(FilterValues.Owned));
		}

		public void ShowAllTanks()
		{
			EventManager.Instance.RaiseEvent(new FiltersChangedEvent(FilterValues.Tank));
		}
	}
}