using System;
using Utils.ConnectionManager;

namespace Calculator
{
    internal class Program
    {
        public static void Main(string[] args)
        {
	        var calculator = new ClientConnector("127.0.0.1", 3004);
	        calculator.Connect("127.0.0.1", 3003);
	        Console.WriteLine("Press a key to exit ...");
	        Console.ReadLine();
        }
    }
}