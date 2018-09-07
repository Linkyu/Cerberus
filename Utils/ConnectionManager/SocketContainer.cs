using System.Net.Sockets;

namespace Utils.ConnectionManager
{
	/// <summary>
	/// Wrapper around Socket used to group it with Address and Port.
	/// </summary>
	public class SocketContainer
	{
		/// <summary>
		/// Socket used to communicate in the connectors.
		/// </summary>
		internal readonly Socket Socket;
		/// <summary>
		/// IP Address of the Socket.
		/// </summary>
		protected internal readonly string Address;
		/// <summary>
		/// Address Port of the Socket.
		/// </summary>
		protected internal readonly int Port;
		
		/// <summary>
		/// Constructor initializing Address and Port
		/// </summary>
		/// <param name="address">IP Address of the Socket.</param>
		/// <param name="port">Address Port of the Socket.</param>
		public SocketContainer(string address, int port)
		{
			Address = address;
			Port = port;
		}

		/// <summary>
		/// Constructor initializing Socket, Address and Port
		/// </summary>
		/// <param name="socket">Socket to be wrapped</param>
		/// <param name="address">IP Address of the Socket.</param>
		/// <param name="port">Address Port of the Socket.</param>
		public SocketContainer(Socket socket, string address, int port)
		{
			Socket = socket;
			Address = address;
			Port = port;
		}
	}
}
