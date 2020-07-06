using AwsomenautsCardGame.Gameplay.Cards;
using VDFramework.SharedClasses.EventSystem;

namespace AwsomenautsCardGame.Events.Gameplay
{	
	public class CardAttackEvent : VDEvent
	{
		public readonly Card AttackingCard;
		public readonly Card AttackedCard;
		
		public CardAttackEvent(Card attackingCard, Card attackedCard)
		{
			AttackingCard = attackingCard;
			AttackedCard = attackedCard;
		}
	}
}