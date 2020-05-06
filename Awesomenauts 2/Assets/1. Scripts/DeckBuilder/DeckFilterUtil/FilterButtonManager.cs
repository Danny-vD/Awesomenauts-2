using System.Collections.Generic;
using System.Linq;
using Enums.Deckbuilder;
using UnityEngine;
using UnityEngine.UI;
using VDFramework;
using VDFramework.Extensions;

namespace DeckBuilder.DeckFilterUtil
{
	public class FilterButtonManager : BetterMonoBehaviour
	{
		[SerializeField]
		private Sprite uncheckedSprite;

		[SerializeField]
		private Sprite checkedSprite;

		[SerializeField]
		private GameObject buttonPrefab;

		private readonly Dictionary<Button, bool> checkBoxStates = new Dictionary<Button, bool>();
		
		private void Awake()
		{
			foreach (FilterValues filter in default(FilterValues).GetValues()
				.Where(item => item != FilterValues.ShowAll))
			{
				GameObject @object = Instantiate(buttonPrefab, CachedTransform);
				
				Text text = @object.GetComponentInChildren<Text>();
				text.text = $"Show {filter.ToString().InsertSpaceBeforeCapitals()}";
				
				Button button = @object.GetComponentInChildren<Button>();
				checkBoxStates.Add(button, true);
				button.image.sprite = checkedSprite;
				
				button.onClick.AddListener(delegate
				{
					ButtonChangeState(button, filter);
				});
			}
		}

		private void ButtonChangeState(Button button, FilterValues filter)
		{
			bool isChecked = checkBoxStates[button] = !checkBoxStates[button];

			button.image.sprite = isChecked ? checkedSprite : uncheckedSprite;

			if (isChecked)
			{
				DeckFilterManager.AddFilters(filter);
			}
			else
			{
				DeckFilterManager.RemoveFilters(filter);
			}
		}
	}
}