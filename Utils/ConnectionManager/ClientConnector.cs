using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Utils.ConnectionManager
{
	public class ClientConnector : BaseConnector
	{
		private static ManualResetEvent ConnectDone = new ManualResetEvent(false);
		
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
				Console.WriteLine(e);
			}
		}
		
		private void ConnectCallback(IAsyncResult ar)
		{
			var orchestrator = (SocketContainer)ar.AsyncState;
			try
			{
				// Complete the connection.
				orchestrator.Socket.EndConnect(ar);
				Console.WriteLine($"Connecté à l'orchestrateur : { orchestrator.Socket.RemoteEndPoint} ");
				ConnectDone.Set();
				Receive(orchestrator);
			}
			catch (SocketException)
			{
				ConnectDone.Set();
				Console.WriteLine("Hote distant injoignable, nouvel essai dans 3 secondes ...  ");
				Thread.Sleep(3000);
				Connect(orchestrator.Address, orchestrator.Port);
			}
		}

	}
}
