using System.Net.Sockets;

namespace Cerberus.Nodes
{
	public class Calculator : Node
	{
		public Calculator(Socket socket, string address, int port) : base(socket,address,port)
		{
			
		}
	}
}
