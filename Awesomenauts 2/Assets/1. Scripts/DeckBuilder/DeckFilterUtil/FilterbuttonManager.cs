using System.Collections.Generic;
using System.Linq;
using Enums.Character;
using UnityEngine;
using UnityEngine.UI;
using VDFramework;
using VDFramework.Extensions;

namespace DeckBuilder.DeckFilterUtil
{
	public class FilterbuttonManager : BetterMonoBehaviour
	{
		[SerializeField]
		private Sprite uncheckedSprite;

		[SerializeField]
		private Sprite checkedSprite;

		[SerializeField]
		private GameObject buttonPrefab;

		private Dictionary<Button, bool> checkBoxStates = new Dictionary<Button, bool>();
		
		private void Awake()
		{
			foreach (Awesomenaut awesomenaut in default(Awesomenaut).GetValues()
				.Where(item => item != Awesomenaut.All))
			{
				GameObject @object = Instantiate(buttonPrefab, CachedTransform);
				
				Text text = @object.GetComponentInChildren<Text>();
				text.text = awesomenaut.ToString().InsertSpaceBeforeCapitals();
				
				Button button = @object.GetComponentInChildren<Button>();
				checkBoxStates.Add(button, true);
				button.image.sprite = checkedSprite;
				
				button.onClick.AddListener(delegate
				{
					ButtonChangeState(button, awesomenaut);
				});
			}
		}

		private void ButtonChangeState(Button button, Awesomenaut naut)
		{
			bool isChecked = checkBoxStates[button] = !checkBoxStates[button];

			button.image.sprite = isChecked ? checkedSprite : uncheckedSprite;

			if (isChecked)
			{
				DeckFilterManager.AddFilters(naut);
			}
			else
			{
				DeckFilterManager.RemoveFilters(naut);
			}
		}
	}
}