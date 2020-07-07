using AwsomenautsCardGame.Enums.Cards;
using AwsomenautsCardGame.Gameplay.Cards;
using UnityEngine;
using UnityEngine.UI;

namespace AwsomenautsCardGame.UI.Cards
{
	public class CardTextHelper : MonoBehaviour
	{
		public Card RegisteredCard;
		public Text DescriptonText;
		public Slider s;

		public Text AttackText;
		public Text SolarText;
		public Text DefenseText;

		// Start is called before the first frame update
		private void Start()
		{
			Register(RegisteredCard);
		}

		public void Register(Card c)
		{
			c.Statistics.Register(CardPlayerStatType.HP, OnDefChanged, true);
			c.Statistics.Register(CardPlayerStatType.Attack, OnAtkChanged, true);
			c.Statistics.Register(CardPlayerStatType.Solar, OnSolarChange, true);
		}

		private void OnDestroy()
		{
			RegisteredCard?.Statistics.UnregisterAll();
		}

		private void OnSolarChange(object newvalue)
		{
			if (SolarText != null)
				SolarText.text = newvalue == null ? string.Empty : newvalue.ToString();
		}

		private void OnAtkChanged(object value)
		{

			if (AttackText != null)
				AttackText.text = value == null ? string.Empty : value.ToString();
		}

		private void OnDefChanged(object value)
		{
			if (DefenseText != null)
			{
				DefenseText.text = value == null ? string.Empty : value.ToString();
			}
			if (value != null && s != null)
			{
				s.value = (float)(int)value / RegisteredCard.Statistics.GetValue<int>(CardPlayerStatType.MaxHP);
			}
		}

		private void OnDescriptionChanged(object value)
		{
			if (DescriptonText != null)
				DescriptonText.text = value == null ? string.Empty : value.ToString();
		}

		// Update is called once per frame
		private void Update() { }
	}
}