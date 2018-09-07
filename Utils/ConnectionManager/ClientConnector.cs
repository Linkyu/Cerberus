using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Utils.ConnectionManager
{
	/// <inheritdoc />
	/// <summary>
	/// Connector used to handle Async Socket communication as a Client
	/// </summary>
	public class ClientConnector : BaseConnector
	{
		/// <summary>
		/// Event to handle Connect status
		/// </summary>
		private static readonly ManualResetEvent ConnectDone = new ManualResetEvent(false);
		/// <summary>
		/// Event linked to ReceivePacket method
		/// </summary>
		public event Action<Packet, SocketContainer> PacketReceived = delegate { }; 
		
		/// <summary>
		/// Constructor initializing Address and Port
		/// </summary>
		/// <param name="address">IP Address of the Socket.</param>
		/// <param name="port">Address Port of the Socket.</param>
		public ClientConnector(string address, int port) : base(address, port){}
		
		/// <summary>
		/// Method used to initiate a Connection with a distant server trough Socket
		/// </summary>
		/// <param name="address">IP Address of the distant server</param>
		/// <param name="port">Address Port of the distant server</param>
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
		
		/// <summary>
		/// Callback called when async Connect method is done
		/// </summary>
		/// <param name="result">Represents the status of an asynchronous operation.</param>
		private void ConnectCallback(IAsyncResult result)
		{
			var orchestrator = (SocketContainer)result.AsyncState;
			try
			{
				orchestrator.Socket.EndConnect(result);
				Logger.Debug($"ClientConnector - ConnectCallback -> Connected to Orchestrator { orchestrator.Socket.RemoteEndPoint} ");
				ConnectDone.Set();
				Receive(orchestrator);
			}
			catch (SocketException)
			{
				ConnectDone.Set();
				Logger.Debug("ClientConnector - ConnectCallback -> Can't join host, new try in 10s ...  ");
				Thread.Sleep(10000);
				Connect(orchestrator.Address, orchestrator.Port);
			}
		}

		/// <summary>
		/// Method that call PacketReceived event when the connector receive a Packet
		/// </summary>
		/// <param name="packet">Packet received</param>
		/// <param name="socketContainer">SocketContainer of the sender</param>
		protected override void ReceivePacket(Packet packet, SocketContainer socketContainer)
		{
			Logger.Debug("ClientConnector - ReceivePacket");
			PacketReceived(packet, socketContainer);
		}
	}
}
