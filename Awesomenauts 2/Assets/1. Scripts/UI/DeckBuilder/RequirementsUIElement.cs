using VDFramework.SharedClasses.Utility;
using VDFramework.VDUnityFramework.BaseClasses;
using UnityEngine;
using UnityEngine.UI;

namespace AwsomenautsCardGame.UI.DeckBuilder
{
	public class RequirementsUIElement : BetterMonoBehaviour
	{
		[SerializeField]
		private Text text = null;

		[SerializeField, Header("text color on condition fail")]
		private Color failColor = new Color(0.8314f, 0.1333f, 0.1333f, 1); //A nice red

		[SerializeField, Header("Enable on condition fail")]
		private GameObject objectToEnable = null;

		private StringVariableWriter minMaxWriter;

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

			minMaxWriter = new StringVariableWriter(text.text);
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
			if (text && minMaxWriter != null)
			{
				text.text = minMaxWriter.UpdateText(currentAmount, minAmount, maxAmount);
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