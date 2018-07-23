using System;
using System.Net;
using System.Net.Sockets;

namespace Utils.ConnectionManager
{
	public class ServerConnector : BaseConnector
	{
		public ServerConnector(string address, int port) : base(address, port) {}

		public ServerConnector(Socket socket, string address, int port) : base(socket, address, port) {}
		
		public async void Listen()
		{
			var listener = new TcpListener(IPAddress.Parse(Container.Address), Container.Port);
			listener.Start();
			Console.WriteLine($"Orchestrateur : Ecoute du port {Container.Port}");
			while (true)
			{
				var socket = await listener.AcceptSocketAsync();
				var address = ((IPEndPoint)socket.RemoteEndPoint).Address.ToString();
				var port = ((IPEndPoint) socket.RemoteEndPoint).Port;
				var socketContainer = new SocketContainer(socket, address, port);
				Console.WriteLine($"Connexion acceptée depuis l'adresse {socket.RemoteEndPoint}");
				IdentifyNode(socketContainer);
				Receive(socketContainer);
			}
		}

		private void IdentifyNode(SocketContainer socketContainer)
		{
			var packet = new Packet(Commands.Identify, null, null);
			Send(socketContainer,packet);
		}
	}
}
