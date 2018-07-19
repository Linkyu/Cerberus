using Cerberus.Nodes;

namespace Cerberus
{
	internal class Program
	{
		public static void Main()
		{
			const string localAddr = "127.0.0.1";
			const int orchPort = 3333;
			var orchestrator = new Orchestrator(localAddr, orchPort);

			var node = new Node(localAddr, 3334);
			node.Connect(localAddr, orchPort);
		}
	}
}
