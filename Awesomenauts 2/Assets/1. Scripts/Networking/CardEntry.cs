using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets._1._Scripts.ScriptableObjects.Effects;
using Player;
using Enums.Cards;
using Enums.Character;
using MasterServer.Common;
using Networking.NetworkingHacks;
using Networking.Statistics;
using Structs.Deckbuilder;
using UnityEngine;

namespace Networking
{
	

	/// <summary>
	/// Info Object for a Card
	/// </summary>
	[Serializable]
	public struct CardEntry
	{
		//Inspector Fields
		//Stats/Designs/etc
		public bool InternalCard;
		public EntityStatistics Statistics;
		public GameObject Prefab;
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
			if (!SerializerSingleton.Serializer.TryWritePacket(ms, Statistics)) throw new Exception("Serializer Write Error");

			return ms.GetBuffer().Take((int) ms.Position).ToArray();

		}
		
		public static EntityStatistics FromNetwork(byte[] buffer)
		{
			InitializeSerializer();
			MemoryStream ms = new MemoryStream(buffer) {Position = 0};
			bool ret = SerializerSingleton.Serializer.TryReadPacket(ms, out EntityStatistics stat);
			if (!ret) throw new Exception("Read packet Exception");

			return stat;
			
		}
	}
}