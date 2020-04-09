using Networking;
using UnityEngine;
using VDFramework;

namespace DeckBuilder
{	
	public class AddAllCards : BetterMonoBehaviour
	{
		[SerializeField]
		private GameObject[] cardParents = new GameObject[0];

		private void Start()
		{
			foreach (GameObject cardParent in cardParents)
			{
				AddCardsAsChild(cardParent);
			}
		}

		private static void AddCardsAsChild(GameObject parent)
		{
			Transform parentTransform = parent.transform;

			foreach (CardEntry entry in CardNetworkManager.Instance.CardEntries)
			{
				InstantiateCardEntry(entry, parentTransform);
			}
		}

		private static void InstantiateCardEntry(CardEntry entry, Transform parent)
		{
			if (entry.Prefab == null)
			{
				return;
			}
				
				
			Instantiate(entry.Prefab, parent);
		}
	}
}