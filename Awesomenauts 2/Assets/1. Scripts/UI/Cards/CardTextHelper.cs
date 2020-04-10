using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Cards
{
	public class CardTextHelper : MonoBehaviour
	{
		public Card RegisteredCard;
		public Text DescriptonText;

		public Text AttackText;

		public Text DefenseText;

		// Start is called before the first frame update
		private void Start()
		{
			RegisteredCard.Statistics.Register(CardPlayerStatType.HP, OnDefChanged, true);
			RegisteredCard.Statistics.Register(CardPlayerStatType.Attack, OnAtkChanged, true);
			RegisteredCard.Statistics.Register(CardPlayerStatType.Description, OnDescriptionChanged, true);
		}

		private void OnAtkChanged(object value)
		{
			AttackText.text = value.ToString();
		}

		private void OnDefChanged(object value)
		{
			DefenseText.text = value.ToString();
		}

		private void OnDescriptionChanged(object value)
		{
			DescriptonText.text = value.ToString();
		}

		// Update is called once per frame
		private void Update() { }
	}
}