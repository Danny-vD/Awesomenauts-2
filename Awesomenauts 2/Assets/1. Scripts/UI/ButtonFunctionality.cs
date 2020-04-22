using DeckBuilder;
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

		//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\
		//		Temporary filter methods
		//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\
		
		public void ShowAllCards()
		{
			DeckFilter.SetFilters(FilterValues.ShowAll);
		}

		public void ShowAllOwned()
		{
			DeckFilter.SetFilters(FilterValues.Owned);
		}

		public void ShowAllTanks()
		{
			DeckFilter.SetFilters(FilterValues.Tank);
		}

		public void ShowAllInDeck()
		{
			DeckFilter.SetFilters(FilterValues.IsIndeck);
		}

		public void ShowAllNotInDeck()
		{
			DeckFilter.SetFilters(FilterValues.IsNotInDeck);
		}

		public void ShowBothDeckAndNot()
		{
			DeckFilter.SetFilters(FilterValues.IsIndeck, FilterValues.IsNotInDeck);
		}
		
		//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\
		//		Temporary sorting methods
		//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\

		public void SortByName()
		{
			DeckSorter.SetSortings(SortValue.Name);
		}

		public void SortByDeck()
		{
			DeckSorter.SetSortings(SortValue.IsInDeck);
		}
		
		public void SortByAmount()
		{
			DeckSorter.SetSortings(SortValue.Amount);
		}
		
		public void SortByDeckAndName()
		{
			DeckSorter.SetSortings(SortValue.IsInDeck, SortValue.Name);
		}

		public void SortByDeckandAmount()
		{
			DeckSorter.SetSortings(SortValue.IsInDeck, SortValue.Amount);
		}

		public void SortByType()
		{
			DeckSorter.SetSortings(SortValue.Type);
		}
	}
}