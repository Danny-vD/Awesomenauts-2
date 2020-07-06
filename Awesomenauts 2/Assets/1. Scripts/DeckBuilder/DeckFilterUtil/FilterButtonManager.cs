using System.Collections.Generic;
using System.Linq;
using VDFramework.SharedClasses.Extensions;
using VDFramework.VDUnityFramework.BaseClasses;
using AwsomenautsCardGame.Enums.Deckbuilder;
using AwsomenautsCardGame.Structs.Deckbuilder;
using UnityEngine;
using UnityEngine.UI;

namespace AwsomenautsCardGame.DeckBuilder.DeckFilterUtil
{
	public class FilterButtonManager : BetterMonoBehaviour
	{
		[SerializeField]
		private Sprite uncheckedSprite = null;

		[SerializeField]
		private Sprite checkedSprite = null;

		[SerializeField]
		private GameObject buttonPrefab = null;

		[SerializeField]
		[Tooltip("What color should \"Show\" prefix have?")]
		private string PrefixColorHex = "A4B7BBFF";

		[SerializeField]
		private List<SpritePerEnum> spritePerEnums = null;

		private readonly Dictionary<Button, bool> checkBoxStates = new Dictionary<Button, bool>();
		
		private void Awake()
		{
			foreach (FilterValues filter in default(FilterValues).GetValues()
				.Where(item => item != FilterValues.ShowAll))
			{
				GameObject @object = Instantiate(buttonPrefab, CachedTransform);
				
				Text text = @object.GetComponentInChildren<Text>();
				text.text = $"<color=#{PrefixColorHex}>Show</color> {filter.ToString().InsertSpaceBeforeCapitals()}";

				Sprite sprite = GetSprite(filter);
				
				if (sprite)
				{
					Image image = @object.GetComponentInChildren<Image>();
					image.enabled = true;
					image.sprite = sprite;
				}
				
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
		
		private Sprite GetSprite(FilterValues filterValues)
		{
			Sprite sprite = spritePerEnums.FirstOrDefault(item => (item.Key & filterValues) != 0).Value;

			return sprite;
		}
	}
}