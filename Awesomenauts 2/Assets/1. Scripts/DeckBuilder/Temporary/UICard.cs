using UnityEngine;
using UnityEngine.UI;
using VDFramework;

namespace DeckBuilder.Temporary
{
	public class UICard : BetterMonoBehaviour
	{
		[SerializeField]
		private Sprite cardSprite = null;

		[SerializeField]
		private CardType type;
		
		private Image image;
		
		private void Start()
		{
			image = GetComponent<Image>();
			
			SetSprite(cardSprite);
		}

		private void OnValidate()
		{
			if (image)
			{
				SetSprite(cardSprite);
			}
			else
			{
				Start();
			}
		}

		public void SetSprite(Sprite sprite)
		{
			image.sprite = sprite;
		}
	}
}