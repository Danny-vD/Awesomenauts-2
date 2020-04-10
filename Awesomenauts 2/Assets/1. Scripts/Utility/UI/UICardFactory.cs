using DeckBuilder;
using UI.Cards;
using UnityEngine;
using VDFramework.Singleton;

namespace Utility.UI
{
	public class UICardFactory : Singleton<UICardFactory>
	{
		[SerializeField]
		private GameObject prefab;

		public AbstractUICard CreateNewCard<TAbstractUICard>(Transform parent, int id)
			where TAbstractUICard : AbstractUICard
		{
			AbstractUICard card = Instantiate(prefab, parent).AddComponent<TAbstractUICard>();
			card.ID = id;
			card.Amount = 1;

			return card;
		}
	}
}