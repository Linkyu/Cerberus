using System.Net.Sockets;
using System.Threading;

namespace Utils.ConnectionManager
{
	public class SocketContainer
	{
		internal Socket Socket;
		protected internal string Address;
		protected internal int Port;
		private static ManualResetEvent SendDone = new ManualResetEvent(false);
		private static ManualResetEvent ConnectDone = new ManualResetEvent(false);
		
		public SocketContainer(string address, int port)
		{
			Address = address;
			Port = port;
		}

		public SocketContainer(Socket socket, string address, int port)
		{
			Socket = socket;
			Address = address;
			Port = port;
		}
	}
}
