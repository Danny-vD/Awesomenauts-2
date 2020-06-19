using Player;
using VDFramework.EventSystem;

namespace Events.Gameplay
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