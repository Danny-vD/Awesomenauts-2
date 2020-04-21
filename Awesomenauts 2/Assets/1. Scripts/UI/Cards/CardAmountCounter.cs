using UnityEngine;
using UnityEngine.UI;
using VDFramework;

namespace UI.Cards
{
	[RequireComponent(typeof(Text))]
	public class CardAmountCounter : BetterMonoBehaviour
	{
		public int Amount
		{
			set => SetText(value);
		}

		private Text amountCounter;

		private void Awake()
		{
			amountCounter = GetComponent<Text>();
		}

		private void SetText(int amount)
		{
			amountCounter.text = amount.ToString();

			IconVisibility(amount > 0);
		}

		private void IconVisibility(bool shouldShow)
		{
			CachedTransform.parent.gameObject.SetActive(shouldShow);
		}
	}
}