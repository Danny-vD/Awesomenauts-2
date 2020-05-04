using DeckBuilder;
using Enums.Deckbuilder;
using VDFramework;
using UnityEngine.SceneManagement;

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

		public void ShowAllAction()
		{
			DeckFilter.SetFilters(FilterValues.Action);
		}
		
		public void ShowAllMelee()
		{
			DeckFilter.SetFilters(FilterValues.Melee);
		}
		
		public void ShowAllRanged()
		{
			DeckFilter.SetFilters(FilterValues.Ranged);
		}		
		
		public void ShowAllTanks()
		{
			DeckFilter.SetFilters(FilterValues.Tank);
		}
		
		public void ShowAllMinion()
		{
			DeckFilter.SetFilters(FilterValues.Melee, FilterValues.Ranged, FilterValues.Tank);
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

		public void SortByTypeAndName()
		{
			DeckSorter.SetSortings(SortValue.Type, SortValue.Name);
		}
		
		public void SortByTypeDeckAndName()
		{
			DeckSorter.SetSortings(SortValue.Type, SortValue.Name, SortValue.IsInDeck);
		}
	}
}