using Events.Deckbuilder;
using UnityEngine;
using UnityEngine.UI;
using VDFramework;
using VDFramework.EventSystem;

namespace DeckBuilder
{
	public class CardPreview : BetterMonoBehaviour
	{
		private Text text;
		private Image image;

		private void Awake()
		{
			text = GetComponentInChildren<Text>();
			image = GetComponentInChildren<Image>();
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

		private void OnHoverUICard(HoverUICardEvent hoverUICardEvent)
		{
			SetPreview(hoverUICardEvent.Card.Sprite);
		}

		private void SetPreview(Sprite sprite)
		{
			image.sprite = sprite;
			text.text = sprite.name;
		}
	}
}