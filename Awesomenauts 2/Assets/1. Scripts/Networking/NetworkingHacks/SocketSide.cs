using System;
using System.Collections.Generic;
using Maps;

[Flags]
public enum SocketSide
{
	SideA = 1,
	SideB = 2,
	RedSide = 4,
	BlueSide = 8,
	NonPlacable = 16,
	Neutral = 32,
}


public static class Lane
{
	public static readonly Dictionary<SocketSide, List<CardSocket>> Sockets = new Dictionary<SocketSide, List<CardSocket>>();

	public static void AddSocket(CardSocket socket)
	{
		if (Sockets.ContainsKey(socket.SocketSide))
		{
			Sockets[socket.SocketSide].Add(socket);
		}
		else
		{
			Sockets.Add(socket.SocketSide, new List<CardSocket> { socket });
		}
	}
}