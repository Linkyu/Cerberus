using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Utils.ConnectionManager
{
    public class StateObject
	{
		public readonly SocketContainer SocketContainer;
		public const int BufferSize = 4096;
		public byte[] Buffer = new byte[BufferSize];
		public List<byte[]> Data = new List<byte[]>();

		public StateObject(SocketContainer socketContainer)
		{
			SocketContainer = socketContainer;
		} 
	}
	public class BaseConnector
	{
		protected readonly SocketContainer Container;
		private static ManualResetEvent SendDone = new ManualResetEvent(false);
		
		public BaseConnector(string address, int port)
		{
			Container = new SocketContainer(address, port);
		}

		public BaseConnector(Socket socket, string address, int port)
		{
			Container = new SocketContainer(socket, address, port);
		}

		protected void Send(SocketContainer socketContainer, Packet packet)
		{
			var data = packet.Serialize();
			try
			{
				socketContainer.Socket.BeginSend(data, 0, data.Length, 0, SendCallback, socketContainer);
			}
			catch (SocketException e)
			{
				// Client Down 
				if (!socketContainer.Socket.Connected)
				{
					Console.WriteLine(e);
					Console.WriteLine($"Client {socketContainer.Socket.RemoteEndPoint} disconnected");
					socketContainer.Socket.Close();
					// TODO => Remove dead node
				}
			}
		}

		private void SendCallback(IAsyncResult ar)
		{
			try
			{
				var socketContainer = (SocketContainer)ar.AsyncState;
				var client = socketContainer.Socket;
				client.EndSend(ar);
				SendDone.Set();
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}

		protected void Receive(SocketContainer socketContainer)
		{
			try
			{
				var stateObj = new StateObject(socketContainer);
				socketContainer.Socket.BeginReceive(stateObj.Buffer, 0, StateObject.BufferSize, 0, ReceiveCallback, stateObj);
			}
			catch (Exception e)
			{
				Console.Write(e);
			}
		}

		private void ReceiveCallback(IAsyncResult result)
		{
			try
			{
				// Retrieve the state object and the client socket 
				// from the asynchronous state object.
				var stateObj = (StateObject) result.AsyncState;
				var socketContainer = stateObj.SocketContainer;
				try
				{
					var nbByteReceived = socketContainer.Socket.EndReceive(result);
					var dataToConcat = new byte[nbByteReceived];
					Array.Copy(stateObj.Buffer, 0, dataToConcat, 0, nbByteReceived);
					stateObj.Data.Add(dataToConcat);
					if (IsEndOfMessage(stateObj.Buffer, nbByteReceived))
					{
						var data = ConcatByteArray(stateObj.Data);
						var packet = Packet.Deserialize(data);
						// TODO => Rename to ReiciveResponse????
						Receive(socketContainer);
						ProcessPacket(packet, socketContainer);
					}
					else
					{
						socketContainer.Socket.BeginReceive(stateObj.Buffer, 0, StateObject.BufferSize, 0, ReceiveCallback, stateObj);
					}
				}
				catch (SocketException e)
				{
					Console.WriteLine(e);
					// TODO => Remove dead node
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}
		
		private static bool IsEndOfMessage(byte[] buffer, int byteRead)
		{
			var endSequence = Encoding.ASCII.GetBytes("ENDSEQ");
			var endOfBuffer = new byte[6];
			Array.Copy(buffer, byteRead - endSequence.Length, endOfBuffer, 0, endSequence.Length);
			return endSequence.SequenceEqual(endOfBuffer);
		}
		
		private byte[] ConcatByteArray(IReadOnlyCollection<byte[]> list)
		{
			var result = new byte[list.Sum(a => a.Length)];
			using(var stream = new MemoryStream(result))
			{
				foreach (var bytes in list)
				{
					stream.Write(bytes, 0, bytes.Length);
				}
			}
			return result;
		}

		private void ProcessPacket(Packet packet, SocketContainer socketContainer)
		{
			// TODO => Process Command
			// Read packet
			// Send(socketContainer, responsePacket);
		}


}
}