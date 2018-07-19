using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Cerberus.Network;

namespace Cerberus.Nodes
{
	public class StateObject
	{
		public Node Node;
		public const int BufferSize = 4096;
		public byte[] Buffer = new byte[BufferSize];
		public List<byte[]> Data = new List<byte[]>();

		public StateObject(Node node)
		{
			Node = node;
		} 
	}
	public class Node
	{
		private Socket Socket;
		protected string Address;
		protected int Port;
		private static ManualResetEvent SendDone = new ManualResetEvent(false);
		private static ManualResetEvent ConnectDone = new ManualResetEvent(false);
		
		public Node(string address, int port)
		{
			Address = address;
			Port = port;
		}

		public Node(Socket socket, string address, int port)
		{
			Socket = socket;
			Address = address;
			Port = port;
		}
		
		public void Connect(string address, int port)
		{
			var endPoint = new IPEndPoint(IPAddress.Parse(address), port);
			var socket = new Socket(endPoint.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			var node = new Node(socket, address, port);
			try
			{
				socket.BeginConnect(endPoint, ConnectCallback, node);
				ConnectDone.WaitOne();
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}
		
		private void ConnectCallback(IAsyncResult ar)
		{
			var orchestrator = (Node)ar.AsyncState;
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

		protected void Send(Node node, Packet packet)
		{
			var data = packet.Serialize();
			try
			{
				node.Socket.BeginSend(data, 0, data.Length, 0, SendCallback, node);
			}
			catch (SocketException e)
			{
				// Client Down 
				if (!node.Socket.Connected)
				{
					Console.WriteLine(e);
					Console.WriteLine($"Client {node.Socket.RemoteEndPoint} disconnected");
					node.Socket.Close();
					// TODO => Remove dead node
				}
			}
		}

		private void SendCallback(IAsyncResult ar)
		{
			try
			{
				var node = (Node)ar.AsyncState;
				var client = node.Socket;
				client.EndSend(ar);
				SendDone.Set();
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}

		protected void Receive(Node node)
		{
			try
			{
				var stateObj = new StateObject(node);
				node.Socket.BeginReceive(stateObj.Buffer, 0, StateObject.BufferSize, 0, ReceiveCallback, stateObj);
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
				var node = stateObj.Node;
				try
				{
					var nbByteReceived = node.Socket.EndReceive(result);
					var dataToConcat = new byte[nbByteReceived];
					Array.Copy(stateObj.Buffer, 0, dataToConcat, 0, nbByteReceived);
					stateObj.Data.Add(dataToConcat);
					if (IsEndOfMessage(stateObj.Buffer, nbByteReceived))
					{
						var data = ConcatByteArray(stateObj.Data);
						var packet = Packet.Deserialize(data);
						// TODO => Rename to ReiciveResponse????
						Receive(node);
						ProcessPacket(packet, node);
					}
					else
					{
						node.Socket.BeginReceive(stateObj.Buffer, 0, StateObject.BufferSize, 0, ReceiveCallback, stateObj);
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

		private void ProcessPacket(Packet packet, Node node)
		{
			// TODO => Process Command
			// Read packet
			// Send(node, responsePacket);
		}
	}
}
