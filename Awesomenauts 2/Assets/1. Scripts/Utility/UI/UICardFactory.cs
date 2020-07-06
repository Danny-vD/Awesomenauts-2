using AwsomenautsCardGame.Enums.Cards;
using AwsomenautsCardGame.Enums.Character;
using AwsomenautsCardGame.Enums.Deckbuilder;
using AwsomenautsCardGame.UI.Cards;
using VDFramework.VDUnityFramework.Singleton;
using UnityEngine;

namespace AwsomenautsCardGame.Utility.UI
{
	public class UICardFactory : Singleton<UICardFactory>
	{
		[SerializeField]
		private GameObject prefab = null;

		public AbstractUICard CreateNewCard<TAbstractUICard>(Transform parent, int id, CardType cardType,
			Awesomenaut awesomenaut, FilterValues filterValues = 0)
			where TAbstractUICard : AbstractUICard
		{
			AbstractUICard card = Instantiate(prefab, parent).AddComponent<TAbstractUICard>();
			card.Filters = filterValues;
			card.Type = cardType;
			card.Awesomenaut = awesomenaut == 0 ? Awesomenaut.All : awesomenaut;

			card.ID = id;

			return card;
		}
	}
}