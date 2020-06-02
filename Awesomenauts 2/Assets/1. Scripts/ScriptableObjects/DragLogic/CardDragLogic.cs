using System;
using Enums.Cards;
using Maps;
using Player;
using UnityEngine;

namespace Assets._1._Scripts.ScriptableObjects.DragLogic
{
	[CreateAssetMenu(menuName = "Scriptable Objects/DragLogic/DefaultLogic")]
	public class CardDragLogic : ScriptableObject
	{

		public SocketTarget Target;
		public int Range;
		public int CrossLaneRange;

		private bool OwnSockets => (Target & SocketTarget.OwnSockets) != 0;
		private bool EnemySockets => (Target & SocketTarget.EnemySockets) != 0;
		private bool NeutralSockets => (Target & SocketTarget.NeutralSockets) != 0;
		private bool EmptySockets => (Target & SocketTarget.Empty) != 0;
		private bool OccupiedSockets => (Target & SocketTarget.Occupied) != 0;


		public virtual CardAction GetAction(CardPlayer player, CardSocket socket, CardSocket socketOfDraggedCard)
		{
			if (!CanTarget(player, socket, socketOfDraggedCard))
				return CardAction.None; //Failsave
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
			if (socketOfDraggedCard != null && Range > 0 && !CanReach(socket, socketOfDraggedCard)) return false;
			if (socket.HasCard && EmptySockets && !OccupiedSockets) return false;
			if (!socket.HasCard && !EmptySockets && OccupiedSockets) return false;
			if (OwnSockets && !EnemySockets && !NeutralSockets && player.ClientID != socket.ClientID) return false;
			if (!OwnSockets && EnemySockets && !NeutralSockets && player.ClientID == socket.ClientID) return false;
			if (!OwnSockets && !EnemySockets && NeutralSockets && socket.ClientID != -1) return false;
			return true;
		}

		private bool CanReach(CardSocket socket, CardSocket socketOfDraggedCard)
		{
			int idx = Lane.Sockets[socketOfDraggedCard.SocketSide].IndexOf(socketOfDraggedCard);
			if (idx == -1) throw new Exception("Socket with invalid index.");
			

			if ((socket.SocketSide & SocketSide.SideA) == (socketOfDraggedCard.SocketSide & SocketSide.SideA)) //They are the same side
			{
				int distance = 0;
				int otherIdx = Lane.Sockets[socketOfDraggedCard.SocketSide].IndexOf(socket);
				if (otherIdx == -1)
				{
					distance += Lane.Sockets[socketOfDraggedCard.SocketSide].Count - 1 - idx; //-1 because we need to get over the array boundary
					otherIdx = Lane.Sockets[socket.SocketSide].IndexOf(socket);
					distance += Mathf.Abs(Lane.Sockets[socket.SocketSide].Count - otherIdx);
				}
				else
				{
					distance += Mathf.Abs(otherIdx - idx);
				}

				return distance <= Range;
			}
			else
			{
				int distance = 0;
				int otherIdx = Lane.Sockets[socketOfDraggedCard.SocketSide].IndexOf(socket);
				if (otherIdx == -1)
				{
					distance += Lane.Sockets[socketOfDraggedCard.SocketSide].Count - 1 - idx; //-1 because we need to get over the array boundary
					otherIdx = Lane.Sockets[socket.SocketSide].IndexOf(socket);
					distance += Mathf.Abs(Lane.Sockets[socket.SocketSide].Count - otherIdx);
				}
				else
				{
					distance += Mathf.Abs(otherIdx - idx);
				}

				return distance <= CrossLaneRange;
				//TODO: Ranger Attack Logic (whatever this means)
			}



		}
	}

}