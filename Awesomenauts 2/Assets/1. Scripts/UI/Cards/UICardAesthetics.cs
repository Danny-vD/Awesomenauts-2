using System.Collections.Generic;
using System.Linq;
using Player;
using Structs.Deckbuilder;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VDFramework;
using VDFramework.Utility;

namespace UI.Cards
{
	public class UICardAesthetics : BetterMonoBehaviour
	{
		[HideInInspector]
		public Image Border = null;

		[HideInInspector]
		public Image Body = null;

		[HideInInspector]
		public Image Portrait = null;

		private readonly Dictionary<Text, StringVariableWriter> writerPerString =
			new Dictionary<Text, StringVariableWriter>();

		private void Awake()
		{
			foreach (Text text in GetComponentsInChildren<Text>())
			{
				writerPerString.Add(text, new StringVariableWriter(text.text));
			}
		}

		public void Initialise(EntityStatistics stats, CardSprites sprites)
		{
			SetAllSprites(sprites);
			SetAllText(stats);
		}

		public void Initialise(UICardAesthetics cardAesthetics)
		{
			SetAllSprites(cardAesthetics);
			SetAllText(cardAesthetics.writerPerString);
		}

		private void UpdateAllText(params object[] variables)
		{
			foreach (KeyValuePair<Text, StringVariableWriter> pair in writerPerString)
			{
				pair.Key.text = pair.Value.UpdateText(variables);
			}
		}
		
		private void SetAllText(EntityStatistics stats)
		{
			UpdateAllText(stats.GetValue(CardPlayerStatType.Description),
				stats.GetValue(CardPlayerStatType.Attack), stats.GetValue(CardPlayerStatType.HP),
				stats.GetValue(CardPlayerStatType.CardName));
		}

		private void SetAllText(Dictionary<Text, StringVariableWriter> texts)
		{
			for (int i = 0; i < writerPerString.Count; i++)
			{
				writerPerString.ElementAt(i).Key.text = texts.ElementAt(i).Key.text;
			}
		}

		private void SetAllSprites(CardSprites sprites)
		{
			Border.sprite = sprites.Border;
			Body.sprite = sprites.Body;
			Portrait.sprite = sprites.Portrait;
		}
		
		private void SetAllSprites(UICardAesthetics cardAesthetics)
		{
			Border.sprite = cardAesthetics.Border.sprite;
			Body.sprite = cardAesthetics.Body.sprite;
			Portrait.sprite = cardAesthetics.Portrait.sprite;
		}
	}
}