using System;
using System.Net.Sockets;

namespace Calculator
{
    internal static class Calculator
    {
        public static void Main()
        {
            const string server = "127.0.0.1";
            const int port = 13000;
            try 
            {
                var client = new TcpClient(server, port);
        
                Console.WriteLine("\n Press Enter to close connection...");
                Console.Read();
                client.Close();       
            } 
            catch (ArgumentNullException e) 
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            } 
            catch (SocketException e) 
            {
                Console.WriteLine("SocketException: {0}", e);
            }
   
        }
    }
}