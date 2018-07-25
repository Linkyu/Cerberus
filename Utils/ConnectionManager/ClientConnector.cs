using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Threading;

namespace Utils.ConnectionManager
{
	public class ClientConnector : BaseConnector
	{
		private static readonly ManualResetEvent ConnectDone = new ManualResetEvent(false);
		public event Action<Packet, SocketContainer> PacketReceived = delegate { }; 
		
		public ClientConnector(string address, int port) : base(address, port)
		{
		}

		public ClientConnector(Socket socket, string address, int port) : base(socket, address, port)
		{
		}
		
		public void Connect(string address, int port)
		{
			var endPoint = new IPEndPoint(IPAddress.Parse(address), port);
			var socket = new Socket(endPoint.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			var socketContainer = new SocketContainer(socket, address, port);
			try
			{
				socket.BeginConnect(endPoint, ConnectCallback, socketContainer);
				ConnectDone.WaitOne();
			}
			catch (Exception e)
			{
				Logger.Debug(e.ToString());
			}
		}
		
		private void ConnectCallback(IAsyncResult ar)
		{
			var orchestrator = (SocketContainer)ar.AsyncState;
			try
			{
				// Complete the connection.
				orchestrator.Socket.EndConnect(ar);
				Logger.Debug($"Connected to Orchestrator { orchestrator.Socket.RemoteEndPoint} ");
				ConnectDone.Set();
				Receive(orchestrator);
			}
			catch (SocketException)
			{
				ConnectDone.Set();
				Logger.Debug("Can't join host, new try in 10s ...  ");
				Thread.Sleep(10000);
				Connect(orchestrator.Address, orchestrator.Port);
			}
		}

		protected override void ReceivePacket(Packet packet, SocketContainer socketContainer)
		{
			Logger.Debug("ClientConnector - ReceivePacket");
			PacketReceived(packet, socketContainer);
		}
	}
}
