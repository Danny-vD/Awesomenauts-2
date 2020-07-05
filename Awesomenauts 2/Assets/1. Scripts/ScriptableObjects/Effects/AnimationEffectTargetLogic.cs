using System.Linq;
using Maps;
using Player;

namespace Assets._1._Scripts.ScriptableObjects.Effects {
	public static class AnimationEffectTargetLogic
	{
		public static Card[] GetTargets(EffectTarget target, Card sourceCard, CardSocket targetSocket)
		{
			Card[] ret;
			if ((target & EffectTarget.Player) != 0)
			{
				//stats = new[]{CardPlayer
				//	.ServerPlayers[
				//		MapTransformInfo.Instance.SocketManager.TID2CID(
				//			c.Statistics.GetValue<int>(CardPlayerStatType.TeamID))].PlayerStatistics};
				if ((target & EffectTarget.Enemies) != 0)
				{

					ret = CardPlayer.ServerPlayers.Where(x =>
							x.PlayerStatistics.GetValue<int>(CardPlayerStatType.TeamID) !=
							sourceCard.Statistics.GetValue<int>(CardPlayerStatType.TeamID)).Select(x => x.Awsomenaut)
						.ToArray();
				}
				else if ((target & EffectTarget.Allies) != 0)
				{
					int id = sourceCard.Statistics.GetValue<int>(CardPlayerStatType.TeamID);
					ret = new[] { CardPlayer.ServerPlayers[id].Awsomenaut };
				}
				else
				{
					ExceptionViewUI.Instance.SetException(new EffectException("No effect Target Specified in object"));
					return new Card[0];
				}

			}
			else if (target == EffectTarget.TargetSocket)
			{
				ret = new[] { targetSocket.DockedCard };
			}
			else if ((target & EffectTarget.Lane) != 0)
			{
				SocketSide side = targetSocket.SocketSide & (SocketSide.SideA | SocketSide.SideB); //Clean all other sides(red/blue/...)
				if ((target & EffectTarget.Enemies) != 0)
				{
					ret = MapTransformInfo.Instance.SocketManager.GetSocketsOnSide(side).Where(x => x.HasCard && sourceCard.Statistics.GetValue<int>(CardPlayerStatType.TeamID) != x.DockedCard.Statistics.GetValue<int>(CardPlayerStatType.TeamID)).Select(x => x.DockedCard).ToArray();

				}
				else if ((target & EffectTarget.Allies) != 0)
				{
					ret = MapTransformInfo.Instance.SocketManager.GetSocketsOnSide(side).Where(x => x.HasCard && sourceCard.Statistics.GetValue<int>(CardPlayerStatType.TeamID) == x.DockedCard.Statistics.GetValue<int>(CardPlayerStatType.TeamID)).Select(x => x.DockedCard).ToArray();
				}
				else
				{
					ExceptionViewUI.Instance.SetException(new EffectException("No effect Target Specified in object"));
					return new Card[0];
				}
			}
			else if ((target & EffectTarget.Board) != 0)
			{
				if ((target & EffectTarget.Enemies) != 0)
				{
					ret = MapTransformInfo.Instance.SocketManager.GetCardSockets().Where(x => x.HasCard && sourceCard.Statistics.GetValue<int>(CardPlayerStatType.TeamID) != x.DockedCard.Statistics.GetValue<int>(CardPlayerStatType.TeamID)).Select(x => x.DockedCard).ToArray();
				}
				else if ((target & EffectTarget.Allies) != 0)
				{
					ret = MapTransformInfo.Instance.SocketManager.GetCardSockets().Where(x => x.HasCard && sourceCard.Statistics.GetValue<int>(CardPlayerStatType.TeamID) == x.DockedCard.Statistics.GetValue<int>(CardPlayerStatType.TeamID)).Select(x => x.DockedCard).ToArray();
				}
				else
				{
					ExceptionViewUI.Instance.SetException(new EffectException("No effect Target Specified in object"));
					return new Card[0];
				}
			}
			else
			{
				ExceptionViewUI.Instance.SetException(new EffectException("No effect Target Specified in object"));
				return new Card[0];
			}

			return ret;
		}
	}
}