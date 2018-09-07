using System;
using System.Net;
using System.Net.Sockets;

namespace Utils.ConnectionManager
{
	/// <summary>
	/// Connector used to handle Async Socket communication as a Server
	/// </summary>
	public class ServerConnector : BaseConnector
	{
		/// <summary>
		/// Event linked to ReceivePacket method
		/// </summary>
		public event Action<Packet, SocketContainer> PacketReceived = delegate { }; 
		
		public ServerConnector(string address, int port) : base(address, port) {}
		
		/// <summary>
		/// Method used to start a ServerConnector
		/// </summary>
		public async void Listen()
		{
			var listener = new TcpListener(IPAddress.Parse(Container.Address), Container.Port);
			listener.Start();
			Logger.Debug($"ServerConnector - Listen -> Listen on port {Container.Port}");
			while (true)
			{
				var socket = await listener.AcceptSocketAsync();
				var address = ((IPEndPoint)socket.RemoteEndPoint).Address.ToString();
				var port = ((IPEndPoint) socket.RemoteEndPoint).Port;
				var socketContainer = new SocketContainer(socket, address, port);
				Logger.Debug($"ServerConnector - Listen -> Connexion from address {socket.RemoteEndPoint}");
				IdentifyNode(socketContainer);
				Receive(socketContainer);
			}
			// ReSharper disable once FunctionNeverReturns
		}

		/// <summary>
		/// Method used to send an Identify Command through Socket connection
		/// </summary>
		/// <param name="socketContainer">SocketContainer of the Node to identify</param>
		private static void IdentifyNode(SocketContainer socketContainer)
		{
			var packet = new Packet(Command.Identify, null, null);
			Send(packet,socketContainer);
		}

		/// <summary>
		/// Method that call PacketReceived event when the connector receive a Packet
		/// </summary>
		/// <param name="packet">Packet received</param>
		/// <param name="socketContainer">SocketContainer of the sender</param>
		protected override void ReceivePacket(Packet packet, SocketContainer socketContainer)
		{
			Logger.Debug("ServerConnector - ReceivePacket");
			PacketReceived(packet, socketContainer);
		}
	}
}
