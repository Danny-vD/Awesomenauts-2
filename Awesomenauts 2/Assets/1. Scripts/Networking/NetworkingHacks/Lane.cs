using System.Collections.Generic;
using AwsomenautsCardGame.Maps;

namespace AwsomenautsCardGame.Networking.NetworkingHacks {
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
}