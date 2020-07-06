using System;
using AwsomenautsCardGame.Enums.Cards;

namespace AwsomenautsCardGame.DataObjects.Networking
{
	public class NetworkEntityStat
	{
		public CardPlayerStatType StatType;
		public Type ValueType;
		public object Value;
	}

}