using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets._1._Scripts.ScriptableObjects.Effects;
using Player;
using Byt3.Serialization;
using Enums.Cards;
using Enums.Character;
using MasterServer.Common;
using MasterServer.Common.Networking.Packets.Serializers;
using Networking.NetworkingHacks;
using Networking.Statistics;
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

		public EntityStatistics Statistics;
		public GameObject Prefab;
		public Sprite cardSprite;
		public Sprite cardPortrait;
		public BorderInfo cardBorder;
		public List<AEffect> effects;
		public CardType CardType;
		public Awesomenaut Awesomenaut;
		public int Amount;

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