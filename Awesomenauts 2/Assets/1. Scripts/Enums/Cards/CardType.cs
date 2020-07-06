using AwsomenautsCardGame.Enums.Deckbuilder;
using JetBrains.Annotations;

namespace AwsomenautsCardGame.Enums.Cards
{
	public enum CardType
	{
		[UsedImplicitly]
		Action = FilterValues.Action,

		[UsedImplicitly]
		Melee = FilterValues.Melee,

		[UsedImplicitly]
		Ranged = FilterValues.Ranged,

		[UsedImplicitly]
		Tank = FilterValues.Tank,

		[UsedImplicitly]
		ActionNoTarget = FilterValues.NoTargetAction,
	}
}