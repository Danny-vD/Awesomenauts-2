using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets._1._Scripts.ScriptableObjects.Effects;
using Enums.Cards;
using Networking;
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

		[HideInInspector]
		public GameObject Icons = null;

		private readonly Dictionary<Text, StringVariableWriter> writerPerString =
			new Dictionary<Text, StringVariableWriter>();

		private void Awake()
		{
			foreach (Text text in GetComponentsInChildren<Text>())
			{
				writerPerString.Add(text, new StringVariableWriter(text.text));
			}
		}

		public void Initialise(AbstractUICard card, CardEntry entry)
		{
			SetAllSprites(entry.Sprites);
			SetAllText(entry, entry.Statistics);

			Icons.SetActive(card.Type != CardType.Action);
		}

		public void Initialise(UICardAesthetics cardAesthetics)
		{
			SetAllSprites(cardAesthetics);
			SetAllText(cardAesthetics.writerPerString);
			
			Icons.SetActive(cardAesthetics.Icons.activeSelf);
		}

		private void UpdateAllText(params object[] variables)
		{
			foreach (KeyValuePair<Text, StringVariableWriter> pair in writerPerString)
			{
				pair.Key.text = pair.Value.UpdateText(variables);
			}
		}

		private void SetAllText(CardEntry entry, EntityStatistics stats)
		{
			UpdateAllText(GetCardDescription(entry),
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

		private string GetCardDescription(CardEntry entry)
		{
			StringBuilder stringBuilder = new StringBuilder("");
			
			foreach (AEffect effect in entry.effects)
			{
				stringBuilder.Append(effect.Description);
				stringBuilder.Append(Environment.NewLine);
			}

			return stringBuilder.ToString();
		}
	}
}