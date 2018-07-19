using System;
using System.Net;
using System.Net.Sockets;
using Cerberus.Network;

namespace Cerberus.Nodes
{
	public class Orchestrator : Node
	{
		private int nbNodes = 1;
		
		public Orchestrator(string address, int port) : base(address,port)
		{
			Listen();
		}
		
		public Orchestrator(Socket socket,string address, int port) : base(socket,address,port)
		{
			Listen();
		}

		private async void Listen()
		{
			var listener = new TcpListener(IPAddress.Parse(Address), Port);
			listener.Start();
			Console.WriteLine($"Orchestrateur : Ecoute du port {Port}");
			while (true)
			{
				var socket = await listener.AcceptSocketAsync();
				var address = ((IPEndPoint)socket.RemoteEndPoint).Address.ToString();
				var port = ((IPEndPoint) socket.RemoteEndPoint).Port;
				var node = new Node(socket, address, port);
				Console.WriteLine($"Connexion acceptée depuis l'adresse {socket.RemoteEndPoint}");
				IdentifyNode(socket, address, port);
				Receive(node);
				nbNodes++;
			}
		}

		private void IdentifyNode(Socket socket, string address, int port)
		{
			var packet = new Packet(Commands.Identify, null, null);
		}
	}
}
