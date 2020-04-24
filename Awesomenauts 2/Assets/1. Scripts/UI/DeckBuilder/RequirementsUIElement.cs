using UnityEngine;
using UnityEngine.UI;
using VDFramework;

namespace UI.DeckBuilder
{
	public class RequirementsUIElement : BetterMonoBehaviour
	{
		[SerializeField]
		private Text text;

		[SerializeField, Header("text color on condition fail")]
		private Color failColor;

		[SerializeField, Header("Enable on condition fail")]
		private GameObject objectToEnable;

		private string originalString;

		private Color normalColor = default;
		
		private int currentAmount;
		private int minAmount;
		private int maxAmount;

		private void Awake()
		{
			if (!text)
			{
				return;
			}

			originalString = text.text;
			normalColor = text.color;
		}

		public void UpdateMinMax(int min, int max)
		{
			minAmount = min;
			maxAmount = max;
		}

		public void UpdateAmount(int amount)
		{
			currentAmount = amount;
			
			UpdateText();
		}

		public void SetValid(bool isValid)
		{
			ObjectSetActive(!isValid);
			SetTextColor(isValid ? normalColor : failColor);
		}
		
		private void UpdateText()
		{
			if (text)
			{
				text.text = string.Format(originalString, currentAmount, minAmount, maxAmount);
			}
		}

		private void SetTextColor(Color color)
		{
			if (text)
			{
				text.color = color;
			}
		}

		private void ObjectSetActive(bool setActive)
		{
			if (objectToEnable)
			{
				objectToEnable.SetActive(setActive);
			}
		}
	}
}