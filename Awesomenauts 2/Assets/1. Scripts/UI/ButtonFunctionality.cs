using DeckBuilder;
using DeckBuilder.DeckFilterUtil;
using Enums.Character;
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
			DeckFilterManager.SetFilters(FilterValues.ShowAll);
		}

		public void ShowAllOwned()
		{
			DeckFilterManager.SetFilters(FilterValues.Owned);
		}

		public void ShowAllAction()
		{
			DeckFilterManager.SetFilters(FilterValues.Action);
		}
		
		public void ShowAllMelee()
		{
			DeckFilterManager.SetFilters(FilterValues.Melee);
		}
		
		public void ShowAllRanged()
		{
			DeckFilterManager.SetFilters(FilterValues.Ranged);
		}		
		
		public void ShowAllTanks()
		{
			DeckFilterManager.SetFilters(FilterValues.Tank);
		}
		
		public void ShowAllMinion()
		{
			DeckFilterManager.SetFilters(FilterValues.Melee, FilterValues.Ranged, FilterValues.Tank);
		}

		public void ShowAllInDeck()
		{
			DeckFilterManager.SetFilters(FilterValues.IsInDeck);
		}

		public void ShowAllNotInDeck()
		{
			DeckFilterManager.SetFilters(FilterValues.IsNotInDeck);
		}

		public void ShowBothDeckAndNot()
		{
			DeckFilterManager.SetFilters(FilterValues.IsInDeck, FilterValues.IsNotInDeck);
		}

		public void ShowAllAwesomeNaut()
		{
			DeckFilterManager.SetFilters(Awesomenaut.All);
		}

		public void ShowLonestar()
		{
			DeckFilterManager.SetFilters(Awesomenaut.SheriffLonestar);
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