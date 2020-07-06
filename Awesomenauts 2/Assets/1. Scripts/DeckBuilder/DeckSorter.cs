using System.Collections.Generic;
using System.Linq;
using VDFramework.SharedClasses.EventSystem;
using AwsomenautsCardGame.DeckBuilder.DeckSorterUtil;
using AwsomenautsCardGame.Enums.Deckbuilder;
using AwsomenautsCardGame.Events.Deckbuilder;
using AwsomenautsCardGame.UI.Cards;

namespace AwsomenautsCardGame.DeckBuilder
{
	public class DeckSorter
	{
		private readonly List<IEnumerable<AbstractUICard>> collectionsToSort = new List<IEnumerable<AbstractUICard>>();

		private DeckSorterMethods sorterMethods;

		public DeckSorter()
		{
			sorterMethods = new DeckSorterMethods();
			
			AddListeners();
		}

		public void Destroy()
		{
			collectionsToSort.Clear();
			
			sorterMethods.Destroy();
			sorterMethods = null;
			
			RemoveListeners();
		}

		private void AddListeners()
		{
			EventManager.Instance.AddListener<ClickUICardEvent>(OnClickUICard);
			EventManager.Instance.AddListener<SortingsChangedEvent>(OnSortingsChanged, int.MinValue);
		}

		private void RemoveListeners()
		{
			if (!EventManager.IsInitialized)
			{
				return;
			}

			EventManager.Instance.RemoveListener<ClickUICardEvent>(OnClickUICard);
			EventManager.Instance.RemoveListener<SortingsChangedEvent>(OnSortingsChanged);
		}

		public void AddToBeSorted(IEnumerable<AbstractUICard> collection)
		{
			if (collectionsToSort.Contains(collection))
			{
				return;
			}

			collectionsToSort.Add(collection);
		}

		public void RemoveToBeSorted(IEnumerable<AbstractUICard> collection)
		{
			collectionsToSort.Remove(collection);
		}

		public static void SetSortings(params SortValue[] newSortings)
		{
			SetSortings(newSortings.ToList());
		}

		public static void SetSortings(List<SortValue> newSortings)
		{
			EventManager.Instance.RaiseEvent(new SortingsChangedEvent(newSortings));
		}

		private static void SortInHierarchy(IEnumerable<AbstractUICard> cards)
		{
			foreach (AbstractUICard card in cards)
			{
				//Show the list with the first item first
				card.CachedTransform.SetAsLastSibling();
			}
		}

		private void SortCollection(IEnumerable<AbstractUICard> abstractUICards)
		{
			List<AbstractUICard> cards = abstractUICards.ToList();

			cards.Sort(sorterMethods.Sort);

			SortInHierarchy(cards);
		}

		private void SortCollections()
		{
			collectionsToSort.ForEach(SortCollection);
		}

		private void OnSortingsChanged(SortingsChangedEvent sortingsChangedEvent)
		{
			SortCollections();
		}

		private void OnClickUICard()
		{
			SortCollections();
		}
	}
}