using System.Data.SqlClient;
using Maps;
using Player;
using UnityEngine;

namespace Assets._1._Scripts.ScriptableObjects.DragLogic
{
	public enum CardAction { None, Attack, Move }

	[CreateAssetMenu(menuName = "Scriptable Objects/DragLogic/DefaultLogic")]
	public class CardDragLogic : ScriptableObject
	{

		public SocketTarget Target;
		public float Range;




		private float SqrRange => Range * Range;
		private bool OwnSockets => (Target & SocketTarget.OwnSockets) != 0;
		private bool EnemySockets => (Target & SocketTarget.EnemySockets) != 0;
		private bool NeutralSockets => (Target & SocketTarget.NeutralSockets) != 0;
		private bool EmptySockets => (Target & SocketTarget.Empty) != 0;
		private bool OccupiedSockets => (Target & SocketTarget.Occupied) != 0;


		public virtual CardAction GetAction(CardPlayer player, CardSocket socket, CardSocket socketOfDraggedCard)
		{
			if (!CanTarget(player, socket, socketOfDraggedCard)) return CardAction.None; //Failsave
			if (socket.HasCard)
			{
				if (socket.DockedCard.Statistics.GetValue<int>(CardPlayerStatType.TeamID) != player.ClientID)
				{
					return CardAction.Attack;
				}
			}
			else
			{
				return CardAction.Move;
			}
			return CardAction.None;
		}

		public virtual bool CanTarget(CardPlayer player, CardSocket socket, CardSocket socketOfDraggedCard)
		{
			if (socketOfDraggedCard != null && Range > 0 && SqrRange < (socket.transform.position - socketOfDraggedCard.transform.position).sqrMagnitude) return false;
			if (socket.HasCard && EmptySockets && !OccupiedSockets) return false;
			if (!socket.HasCard && !EmptySockets && OccupiedSockets) return false;
			if (OwnSockets && !EnemySockets && !NeutralSockets && player.ClientID != socket.ClientID) return false;
			if (!OwnSockets && EnemySockets && !NeutralSockets && player.ClientID == socket.ClientID) return false;
			if (!OwnSockets && !EnemySockets && NeutralSockets && socket.ClientID != -1) return false;
			return true;
		}
	}

}