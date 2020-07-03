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
			if (/*socket.SocketType == SocketType.Default && */!CanTarget(player, socket, socketOfDraggedCard))
			{
				return CardAction.None; //Failsave
			}

			if (!player.CanUseCard(socketOfDraggedCard.DockedCard))
			{
				return CardAction.None;
			}

			if (socket.HasCard)
			{
				if (socket.DockedCard.Statistics.GetValue<int>(CardPlayerStatType.TeamID) != player.ClientID)
				{
					TooltipScript.Instance.SetTooltip(TooltipType.CardAttackAccepted);
					return CardAction.Attack;
				}

				return CardAction.None;
			}
			else
			{
				TooltipScript.Instance.SetTooltip(TooltipType.CardMovingAccepted);
				return CardAction.Move;
			}
		}

		public virtual bool CanTarget(CardPlayer player, CardSocket socket, CardSocket socketOfDraggedCard)
		{
			if (NoSockets)
			{
				TooltipScript.Instance.SetTooltip(TooltipType.CardCanNotBeTheTarget);
				return false;
			}
			int range = socketOfDraggedCard != null && socketOfDraggedCard.HasCard
				? socketOfDraggedCard.DockedCard.Statistics.GetValue<int>(CardPlayerStatType.Range) : Range;
			int xrange = socketOfDraggedCard != null && socketOfDraggedCard.HasCard
				? socketOfDraggedCard.DockedCard.Statistics.GetValue<int>(CardPlayerStatType.CrossLaneRange) : CrossLaneRange;

			if (socketOfDraggedCard != null && range > 0 /*&& (socket.SocketSide & SocketSide.NonPlacable) != 0*/)
			{
				if (!CanReach(socket, socketOfDraggedCard, range, xrange))
				{
					return false;
				}
			}
			if (socket.HasCard && EmptySockets && !OccupiedSockets)
			{
				TooltipScript.Instance.SetTooltip(TooltipType.CardCanNotBeTheTarget);
				return false;
			}
			if (!socket.HasCard && !EmptySockets && OccupiedSockets)
			{

				TooltipScript.Instance.SetTooltip(TooltipType.CardCanNotBeTheTarget);
				return false;
			}
			if (OwnSockets && !EnemySockets && !NeutralSockets && player.ClientID != socket.ClientID)
			{
				TooltipScript.Instance.SetTooltip(TooltipType.CardCanNotBeTheTarget);
				return false;
			}
			if (!OwnSockets && EnemySockets && !NeutralSockets && player.ClientID == socket.ClientID)
			{
				TooltipScript.Instance.SetTooltip(TooltipType.CardCanNotBeTheTarget);
				return false;
			}
			if (!NeutralSockets && (socket.SocketSide & SocketSide.Neutral) != 0)
			{
				TooltipScript.Instance.SetTooltip(TooltipType.CardCanNotBeTheTarget);
				return false;
			}
			return true;
		}

		private bool CanReach(CardSocket socket, CardSocket socketOfDraggedCard, int range, int xrange)
		{

			//if ((socket.SocketSide & SocketSide.NonPlacable) != 0) return false;
			if ((socket.SocketSide & (SocketSide.SideA | SocketSide.SideB)) != 0 &&
			    (socketOfDraggedCard.SocketSide & (SocketSide.SideA | SocketSide.SideB)) != 0 &&
			    (socket.SocketSide & SocketSide.SideA) != (socketOfDraggedCard.SocketSide & SocketSide.SideA))  //When both sockets belong to a lane and the lanes are different
			{
				if ((socket.SocketSide & (SocketSide.BlueSide | SocketSide.RedSide)) != 0 && //If the cards are one of the sides
					(socketOfDraggedCard.SocketSide & (SocketSide.BlueSide | SocketSide.RedSide)) != 0)
				{
					if ((socket.SocketSide & (SocketSide.BlueSide | SocketSide.RedSide)) == //If the cards are on the same side(Blue/Red).
						(socketOfDraggedCard.SocketSide & (SocketSide.BlueSide | SocketSide.RedSide)))
					{
						if (Mathf.Abs(socket.transform.position.z - socketOfDraggedCard.transform.position.z) < 1) //If the sockets have "almost" the same z component(e.g. directly perpendicular to the lane)
						{
							bool xret = xrange > 0;
							if (!xret)
							{
								TooltipScript.Instance.SetTooltip(TooltipType.CanNotMoveCrossLane);
							}

							return xret;
						}
						else
						{
							TooltipScript.Instance.SetTooltip(TooltipType.CanNotMoveDiagonally);
							return false;
						}
					}
				}
			}
			//if (socket == null || socketOfDraggedCard == null) return true;

			List<CardSocket> path = AStar.AStar.Compute(socketOfDraggedCard, socket);

			//Debug.Log("A* Distance:" + path.Count);



			bool ret= path.Count <= range; //Not factoring in the "cross lane"

			if (!ret)
			{
				TooltipScript.Instance.SetTooltip(TooltipType.NotEnoughRange);
			}

			return ret;

		}
	}

}