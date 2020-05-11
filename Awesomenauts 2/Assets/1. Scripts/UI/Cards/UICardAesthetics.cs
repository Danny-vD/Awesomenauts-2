using System.Collections.Generic;
using System.Linq;
using Player;
using UnityEngine;
using UnityEngine.UI;
using VDFramework;
using VDFramework.Utility;

namespace UI.Cards
{
	public class UICardAesthetics : BetterMonoBehaviour
	{
		//TODO: remove
		private const string temporaryString =
			"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";

		//HideInInspector does not serialise private fields, as it turns out
		[HideInInspector, SerializeField]
		private Image border = null;

		[HideInInspector, SerializeField]
		private Image body = null;

		[HideInInspector, SerializeField]
		private Image portrait = null;

		private readonly Dictionary<Text, StringVariableWriter> writerPerString =
			new Dictionary<Text, StringVariableWriter>();

		private void Awake()
		{
			foreach (Text text in GetComponentsInChildren<Text>())
			{
				writerPerString.Add(text, new StringVariableWriter(text.text));
			}
		}

		public void Initialise(EntityStatistics stats)
		{
			SetAllText(stats);
		}

		public void Initialise(UICardAesthetics cardAesthetics)
		{
			SetAllSprites(cardAesthetics);
			SetAllText(cardAesthetics.writerPerString);
		}

		private void SetAllText(EntityStatistics stats)
		{
			UpdateAllText(temporaryString, stats.GetValue(CardPlayerStatType.Attack),
				stats.GetValue(CardPlayerStatType.HP));
		}

		private void SetAllText(Dictionary<Text, StringVariableWriter> texts)
		{
			for (int i = 0; i < writerPerString.Count; i++)
			{
				writerPerString.ElementAt(i).Key.text = texts.ElementAt(i).Key.text;
			}
		}

		private void UpdateAllText(params object[] variables)
		{
			foreach (KeyValuePair<Text, StringVariableWriter> pair in writerPerString)
			{
				pair.Key.text = pair.Value.UpdateText(variables);
			}
		}

		private void SetAllSprites(UICardAesthetics cardAesthetics)
		{
			border.sprite = cardAesthetics.border.sprite;
			body.sprite = cardAesthetics.body.sprite;
			portrait.sprite = cardAesthetics.portrait.sprite;
		}
	}
}