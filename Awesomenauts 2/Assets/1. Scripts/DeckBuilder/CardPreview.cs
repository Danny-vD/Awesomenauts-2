using Events.Deckbuilder;
using UI.Cards;
using UnityEngine;
using UnityEngine.UI;
using VDFramework;
using VDFramework.EventSystem;

namespace DeckBuilder
{
	public class CardPreview : BetterMonoBehaviour
	{
		[SerializeField]
		private Text cardName;
		
		private UICardAesthetics cardAesthetics;

		private void Awake()
		{
			cardAesthetics = GetComponentInChildren<UICardAesthetics>();
		}

		private void OnEnable()
		{
			AddListeners();
		}

		private void OnDisable()
		{
			RemoveListeners();
		}

		private void AddListeners()
		{
			EventManager.Instance.AddListener<HoverUICardEvent>(OnHoverUICard);
		}

		private void RemoveListeners()
		{
			if (EventManager.IsInitialized)
			{
				EventManager.Instance.RemoveListener<HoverUICardEvent>(OnHoverUICard);
			}
		}
		
		private void SetPreview(AbstractUICard card)
		{
			cardName.text = card.name;
			cardAesthetics.Initialise(card.CardAesthetics);
		}
		
		private void OnHoverUICard(HoverUICardEvent hoverUICardEvent)
		{
			SetPreview(hoverUICardEvent.Card);
		}
	}
}