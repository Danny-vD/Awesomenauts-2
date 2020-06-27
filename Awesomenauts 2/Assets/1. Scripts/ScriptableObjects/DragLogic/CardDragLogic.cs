using System;
using System.Collections.Generic;
using Byt3.Collections;
using Byt3.Collections.Interfaces;
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

		private bool NoSockets => Target == SocketTarget.Nothing;


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
			else if(player.CanMoveCard(socketOfDraggedCard.DockedCard))
			{
				return CardAction.Move;
			}
			return CardAction.None;
		}

		public virtual bool CanTarget(CardPlayer player, CardSocket socket, CardSocket socketOfDraggedCard)
		{
			if (NoSockets) return false;
			int range = socketOfDraggedCard != null && socketOfDraggedCard.HasCard
				? socketOfDraggedCard.DockedCard.Statistics.GetValue<int>(CardPlayerStatType.Range) : Range;
			int xrange = socketOfDraggedCard != null && socketOfDraggedCard.HasCard
				? socketOfDraggedCard.DockedCard.Statistics.GetValue<int>(CardPlayerStatType.CrossLaneRange) : CrossLaneRange;

			if (socketOfDraggedCard != null && range > 0 && !CanReach(socket, socketOfDraggedCard, range, xrange)) return false;
			if (socket.HasCard && EmptySockets && !OccupiedSockets) return false;
			if (!socket.HasCard && !EmptySockets && OccupiedSockets) return false;
			if (OwnSockets && !EnemySockets && !NeutralSockets && player.ClientID != socket.ClientID) return false;
			if (!OwnSockets && EnemySockets && !NeutralSockets && player.ClientID == socket.ClientID) return false;
			if (!NeutralSockets && (socket.SocketSide & SocketSide.Neutral) != 0) return false;
			return true;
		}

		private bool CanReach(CardSocket socket, CardSocket socketOfDraggedCard, int range, int xrange)
		{

			if ((socket.SocketSide & SocketSide.NonPlacable) != 0) return false;
			if ((socket.SocketSide & (SocketSide.SideA | SocketSide.SideB)) != 0 && (socketOfDraggedCard.SocketSide & (SocketSide.SideA | SocketSide.SideB)) != 0 && (socket.SocketSide & SocketSide.SideA) != (socketOfDraggedCard.SocketSide & SocketSide.SideA)) //When both sockets belong to a lane and the lanes are different
			{
				if ((socket.SocketSide & (SocketSide.BlueSide | SocketSide.RedSide)) != 0 && //If the cards are one of the sides
					(socketOfDraggedCard.SocketSide & (SocketSide.BlueSide | SocketSide.RedSide)) != 0)
				{
					if ((socket.SocketSide & (SocketSide.BlueSide | SocketSide.RedSide)) == //If the cards are on the same side(Blue/Red).
						(socketOfDraggedCard.SocketSide & (SocketSide.BlueSide | SocketSide.RedSide)))
					{
						if (Mathf.Abs(socket.transform.position.z - socketOfDraggedCard.transform.position.z) < 1) //If the sockets have "almost" the same z component(e.g. directly perpendicular to the lane)
						{
							return xrange > 0;
						}
					}
				}
			}
			//if (socket == null || socketOfDraggedCard == null) return true;

			List<CardSocket> path = AStar.AStar.Compute(socketOfDraggedCard, socket);

			Debug.Log("A* Distance:" + path.Count);



			return path.Count <= range; //Not factoring in the "cross lane"




		}
	}

}