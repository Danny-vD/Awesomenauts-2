using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AwsomenautsCardGame.Enums.Cards;
using AwsomenautsCardGame.Enums.Character;
using AwsomenautsCardGame.Networking.NetworkingHacks;
using AwsomenautsCardGame.Player;
using AwsomenautsCardGame.ScriptableObjects.Effects;
using AwsomenautsCardGame.Structs.Deckbuilder;
using AwsomenautsCardGame.UI;
using Byt3.Utilities.Exceptions;
using MasterServer.Common;
using UnityEngine;

namespace AwsomenautsCardGame.Networking
{
	/// <summary>
	/// Info Object for a Card
	/// </summary>
	[Serializable]
	public struct CardEntry
	{
		public string name;

		//Inspector Fields
		//Stats/Designs/etc
		[HideInInspector]
		public int index;
		public bool InternalCard;
		public EntityStatistics Statistics;
		public GameObject Prefab;
		public CardModelAsset Model;
		public BorderInfo cardBorder;
		public CardType CardType;
		public CardSprites Sprites;
		public Awesomenaut Awesomenaut;
		public int Amount;
		public List<AEffect> effects;

		//Flag that indicates if the Serializer was initialized.
		private static bool init;


		/// <summary>
		/// Boilerplate for Byt3Serializer
		/// </summary>
		private static void InitializeSerializer()
		{
			if (init) return;

			init = true;
			SerializerSingleton.Serializer.AddSerializer<EntityStatistics>(new EntityStatisticsSerializer());
			SerializerSingleton.Serializer.AddSerializer<NetworkEntityStat>(new EntityStatSerializer());
		}

		public byte[] StatisticsToNetworkableArray()
		{
			InitializeSerializer();
			MemoryStream ms = new MemoryStream();
			if (!SerializerSingleton.Serializer.TryWritePacket(ms, Statistics)) ExceptionViewUI.Instance.SetException(new Byt3Exception("Serializer Write Error"), "Serialization Exception:");

			return ms.GetBuffer().Take((int)ms.Position).ToArray();

		}

		public static EntityStatistics FromNetwork(byte[] buffer)
		{
			InitializeSerializer();
			MemoryStream ms = new MemoryStream(buffer) { Position = 0 };
			bool ret = SerializerSingleton.Serializer.TryReadPacket(ms, out EntityStatistics stat);
			if (!ret) ExceptionViewUI.Instance.SetException(new Byt3Exception("Read packet Exception"), "Deserialization Exception:");

			return stat;

		}
	}
}