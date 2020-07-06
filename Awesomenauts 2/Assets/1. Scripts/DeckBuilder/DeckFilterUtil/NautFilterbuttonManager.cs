using System.Collections.Generic;
using System.Linq;
using AwsomenautsCardGame.Enums.Character;
using VDFramework.SharedClasses.Extensions;
using VDFramework.VDUnityFramework.BaseClasses;
using UnityEngine;
using UnityEngine.UI;

namespace AwsomenautsCardGame.DeckBuilder.DeckFilterUtil
{
	public class NautFilterbuttonManager : BetterMonoBehaviour
	{
		[SerializeField]
		private Sprite uncheckedSprite = null;

		[SerializeField]
		private Sprite checkedSprite = null;

		[SerializeField]
		private GameObject buttonPrefab = null;

		private readonly Dictionary<Button, bool> checkBoxStates = new Dictionary<Button, bool>();
		
		private void Awake()
		{
			Awesomenaut awesomenautFilters = 0;
			
			foreach (Awesomenaut awesomenaut in default(Awesomenaut).GetValues()
				.Where(item => item != Awesomenaut.All))
			{
				awesomenautFilters |= awesomenaut;
				
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
			
			DeckFilterManager.SetFilters(awesomenautFilters);
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