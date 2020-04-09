using Networking;
using UnityEngine;
using Utility.UI;
using VDFramework;

namespace DeckBuilder
{
	public class AddAllCards : BetterMonoBehaviour
	{
		private void Start()
		{
			AddCardsAsChild(CachedTransform);
		}

		private static void AddCardsAsChild(Transform parentTransform)
		{
			int length = CardNetworkManager.Instance.CardEntries.Length;

			for (int id = 0; id < length; id++)
			{
				CardEntry entry = CardNetworkManager.Instance.CardEntries[id];
				InstantiateCardEntry(entry, parentTransform, id);
			}
		}

		private static void InstantiateCardEntry(CardEntry entry, Transform parent, int id)
		{
			if (entry.cardSprite == null)
			{
				return;
			}

			AbstractUICard card = UICardFactory.Instance.CreateNewCard<AvailableUICard>(parent, id);

			card.Sprite = entry.cardSprite;
		}
	}
}