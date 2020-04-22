using Enums.Cards;
using Enums.Deckbuilder;
using UI.Cards;
using UnityEngine;
using VDFramework.Singleton;

namespace Utility.UI
{
	public class UICardFactory : Singleton<UICardFactory>
	{
		[SerializeField]
		private GameObject prefab = null;

		public AbstractUICard CreateNewCard<TAbstractUICard>(Transform parent, int id, CardType cardType, FilterValues filterValues = 0)
			where TAbstractUICard : AbstractUICard
		{
			AbstractUICard card = Instantiate(prefab, parent).AddComponent<TAbstractUICard>();
			card.Filters = filterValues;
			card.Type = cardType;
			
			card.ID = id;

			return card;
		}
	}
}