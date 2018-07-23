using System;
using Utils.ConnectionManager;

namespace Orchestrator
{
    internal class Program
    {
        public static void Main(string[] args)
        {

	        var connectionManager = new ServerConnector("127.0.0.1", 3003);
	        connectionManager.Listen();
	        Console.WriteLine("Press a key to exit ...");
	        Console.ReadLine();
        }
    }
}