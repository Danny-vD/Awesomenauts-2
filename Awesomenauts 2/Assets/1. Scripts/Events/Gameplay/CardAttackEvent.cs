using VDFramework.SharedClasses.EventSystem;
using AwsomenautsCardGame.Player;

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