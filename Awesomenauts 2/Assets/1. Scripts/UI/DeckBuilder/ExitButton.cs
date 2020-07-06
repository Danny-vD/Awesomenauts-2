using VDFramework.SharedClasses.EventSystem;
using VDFramework.VDUnityFramework.BaseClasses;
using VDFramework.VDUnityFramework.UnityExtensions;
using AwsomenautsCardGame.Events.Deckbuilder;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AwsomenautsCardGame.UI.DeckBuilder
{
	[RequireComponent(typeof(Image))]
	public class ExitButton : BetterMonoBehaviour
	{
		[SerializeField]
		private Sprite NormalSprite = null;
		
		[SerializeField]
		private Sprite HoverSprite = null;
		
		[SerializeField]
		private Sprite UnavailableSprite = null;

		private Image image = null;
		
		private bool canClick = true;
		
		private Sprite Sprite
		{
			get => image.sprite;
			set => image.sprite = value;
		}

		private void Awake()
		{
			image = GetComponent<Image>();
			GetComponent<Button>().onClick.AddListener(OnPointerClick);
		}

		private void OnEnable()
		{
			AddListeners();
			AddEventTriggers();
		}

		private void OnDisable()
		{
			RemoveListeners();
		}
		
		private void AddListeners()
		{
			EventManager.Instance.AddListener<ValidDeckEvent>(OnCheckingDeck);
		}
		
		private void RemoveListeners()
		{
			if (!EventManager.IsInitialized)
			{
				return;
			}
    
			EventManager.Instance.RemoveListener<ValidDeckEvent>(OnCheckingDeck);
		}

		private void OnCheckingDeck(ValidDeckEvent validDeckEvent)
		{
			canClick = validDeckEvent.IsValid;

			Sprite = canClick ? NormalSprite : UnavailableSprite;
		}

		private void OnPointerEnter()
		{
			if (canClick)
			{
				Sprite = HoverSprite;
			}
		}

		private void OnPointerClick()
		{
			if (canClick)
			{
				GetComponentInParent<ButtonFunctionality>().LoadScene(1);
			}
		}

		private void OnPointerExit()
		{
			if (canClick)
			{
				Sprite = NormalSprite;
			}
		}
		

		private void AddEventTriggers()
		{
			EventTrigger.Entry pointerEnterEntry =
				new EventTrigger.Entry
					{eventID = EventTriggerType.PointerEnter};
			pointerEnterEntry.callback.AddListener((data) => { OnPointerEnter(); });

			EventTrigger.Entry pointerExitEntry =
				new EventTrigger.Entry
					{eventID = EventTriggerType.PointerExit};
			pointerExitEntry.callback.AddListener((data) => { OnPointerExit(); });

			EventTrigger trigger =
				gameObject.EnsureComponent<EventTrigger>();

			trigger.triggers.Add(pointerEnterEntry);
			trigger.triggers.Add(pointerExitEntry);
		}
	}
}