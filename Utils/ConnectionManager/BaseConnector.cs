using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Utils.ConnectionManager
{
	/// <summary>
	/// Object used to transfer data to callback
	/// </summary>
    public class StateObject
	{
		/// <summary>
		/// Wrapper around the Socket
		/// </summary>
		public readonly SocketContainer SocketContainer;
		/// <summary>
		/// The buffer size used when sending data trough Socket communication
		/// </summary>
		public const int BufferSize = 4096;
		/// <summary>
		/// The buffer used to send data trough Socket communication
		/// </summary>
		public readonly byte[] Buffer = new byte[BufferSize];
		/// <summary>
		/// The Data object used in Socket communication
		/// </summary>
		public readonly List<byte[]> Data = new List<byte[]>();

		/// <summary>
		/// Constructor used to link SocketContainer to StateObject
		/// </summary>
		/// <param name="socketContainer">Wrapper around socket</param>
		public StateObject(SocketContainer socketContainer)
		{
			SocketContainer = socketContainer;
		} 
	}

	/// <summary>
	/// Base Connector used to handle Async Socket communication
	/// </summary>
	public abstract class BaseConnector
	{
		/// <summary>
		/// Wrapper around the Socket
		/// </summary>
		protected readonly SocketContainer Container;
		/// <summary>
		/// Event to handle Send status
		/// </summary>
		private static readonly ManualResetEvent SendDone = new ManualResetEvent(false);

		/// <summary>
		/// Constructor initializing Address and Port
		/// </summary>
		/// <param name="address">IP Address of the Socket.</param>
		/// <param name="port">Address Port of the Socket.</param>
		protected BaseConnector(string address, int port)
		{
			Container = new SocketContainer(address, port);
		}

		/// <summary>
		/// Method used to send Packet through a Socket contained in SocketContainer
		/// </summary>
		/// <param name="packet">Packet to send</param>
		/// <param name="socketContainer">SocketContainer containing the destination Socket</param>
		public static void Send(Packet packet, SocketContainer socketContainer)
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
					Logger.Debug(e.ToString());
					Logger.Debug($"Client {socketContainer.Socket.RemoteEndPoint} disconnected");
					socketContainer.Socket.Close();
					// TODO => Remove dead node
				}
			}
		}

		/// <summary>
		/// Callback called when async Send method is done
		/// </summary>
		/// <param name="result">Represents the status of an asynchronous operation.</param>
		private static void SendCallback(IAsyncResult result)
		{
			try
			{
				var socketContainer = (SocketContainer)result.AsyncState;
				var client = socketContainer.Socket;
				client.EndSend(result);
				SendDone.Set();
			}
			catch (Exception e)
			{
				Logger.Debug(e.ToString());
			}
		}

		/// <summary>
		/// Method used to receive data from async socket connection.
		/// </summary>
		/// <param name="socketContainer">Distant SocketContainer from who we receive the data.</param>
		protected void Receive(SocketContainer socketContainer)
		{
			try
			{
				var stateObj = new StateObject(socketContainer);
				socketContainer.Socket.BeginReceive(stateObj.Buffer, 0, StateObject.BufferSize, 0, ReceiveCallback, stateObj);
			}
			catch (Exception e)
			{
				Logger.Debug(e.ToString());
			}
		}

		/// <summary>
		/// Callback called when we start receiving data.
		/// </summary>
		/// <param name="result">Represents the status of an asynchronous operation.</param>
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
					if (nbByteReceived == 0) return;
					if (IsEndOfMessage(stateObj.Buffer, nbByteReceived))
					{
						var data = ConcatByteArray(stateObj.Data);
						var packet = Packet.Deserialize(data);
						Receive(socketContainer);
						ReceivePacket(packet, socketContainer);
					}
					else
					{
						socketContainer.Socket.BeginReceive(stateObj.Buffer, 0, StateObject.BufferSize, 0, ReceiveCallback, stateObj);
					}
				}
				catch (SocketException e)
				{
					Logger.Debug(e.ToString());
					// TODO => Remove dead node
				}
			}
			catch (Exception e)
			{
				Logger.Debug(e.ToString());
			}
		}
		
		/// <summary>
		/// Method used to check if we have reached the end of the message
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="byteRead"></param>
		/// <returns>Boolean representing if end of message is reached</returns>
		private static bool IsEndOfMessage(byte[] buffer, int byteRead)
		{
			var endSequence = Encoding.ASCII.GetBytes("ENDSEQ");
			var endOfBuffer = new byte[6];
			Array.Copy(buffer, byteRead - endSequence.Length, endOfBuffer, 0, endSequence.Length);
			return endSequence.SequenceEqual(endOfBuffer);
		}
		
		/// <summary>
		/// Concatenate a list of array of byte in one array
		/// </summary>
		/// <param name="list">List to concatenate</param>
		/// <returns>The concatenated array of byte</returns>
		private static byte[] ConcatByteArray(IReadOnlyCollection<byte[]> list)
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

		/// <summary>
		/// Method that call PacketReceived event when the connector receive a Packet
		/// </summary>
		/// <param name="packet">Packet received</param>
		/// <param name="socketContainer">SocketContainer of the sender</param>
		protected virtual void ReceivePacket(Packet packet, SocketContainer socketContainer){}
	}
}