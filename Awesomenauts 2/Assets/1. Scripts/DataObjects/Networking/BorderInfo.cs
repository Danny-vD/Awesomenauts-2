using System;
using AwsomenautsCardGame.Enums.Cards;
using UnityEngine;

namespace AwsomenautsCardGame.DataObjects.Networking {
	[Serializable]
	public class BorderInfo
	{
		public bool IsValid => BorderDefaultMaterial != null && BorderMesh != null;
		public Mesh BorderMesh;
		public Material BorderDefaultMaterial;
		public Material BorderMeleeMaterial;
		public Material BorderRanged;
		public Material BorderTank;
		public Material BorderAction;

		public Material GetMaterial(CardType type)
		{
			switch (type)
			{
				case CardType.Action:
					return BorderAction == null ? BorderDefaultMaterial : BorderAction;
				case CardType.ActionNoTarget:
					return BorderAction == null ? BorderDefaultMaterial : BorderAction;
				case CardType.Melee:
					return BorderMeleeMaterial == null ? BorderDefaultMaterial : BorderMeleeMaterial;
				case CardType.Ranged:
					return BorderRanged == null ? BorderDefaultMaterial : BorderRanged;
				case CardType.Tank:
					return BorderTank == null ? BorderDefaultMaterial : BorderTank;
                default:
	                return BorderDefaultMaterial;
			}
		}
	}
}