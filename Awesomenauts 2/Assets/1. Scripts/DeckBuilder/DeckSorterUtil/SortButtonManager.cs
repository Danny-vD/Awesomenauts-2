using System.Collections.Generic;
using System.Linq;
using DeckBuilder.DeckFilterUtil;
using Enums.Character;
using Enums.Deckbuilder;
using UnityEngine;
using UnityEngine.UI;
using VDFramework;
using VDFramework.Extensions;

namespace DeckBuilder.DeckSorterUtil
{
	public class SortButtonManager : BetterMonoBehaviour
	{
		[SerializeField]
		private Sprite uncheckedSprite = null;

		[SerializeField]
		private Sprite checkedSprite = null;

		[SerializeField]
		private GameObject buttonPrefab = null;

		private readonly Dictionary<Button, bool> checkBoxStates = new Dictionary<Button, bool>();

		private List<SortValue> sortings;
		
		private void Awake()
		{
			sortings = new List<SortValue>();
			
			foreach (SortValue sorting in default(SortValue).GetValues())
			{
				sortings.Add(sorting);
				
				GameObject @object = Instantiate(buttonPrefab, CachedTransform);
				
				Text text = @object.GetComponentInChildren<Text>();
				text.text = sorting.ToString().InsertSpaceBeforeCapitals();
				
				Button button = @object.GetComponentInChildren<Button>();
				checkBoxStates.Add(button, true);
				button.image.sprite = checkedSprite;
				
				button.onClick.AddListener(delegate
				{
					ButtonChangeState(button, sorting);
				});
			}
			
			DeckSorter.SetSortings(sortings);
		}

		private void ButtonChangeState(Button button, SortValue sorting)
		{
			bool isChecked = checkBoxStates[button] = !checkBoxStates[button];

			button.image.sprite = isChecked ? checkedSprite : uncheckedSprite;

			if (isChecked)
			{
				sortings.Add(sorting);
			}
			else
			{
				sortings.Remove(sorting);
			}
			
			DeckSorter.SetSortings(sortings);
		}
	}
}