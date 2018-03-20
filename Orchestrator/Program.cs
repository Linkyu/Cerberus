using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Orchestrator
{
    internal static class Orchestartor
    {
        private static List<Client> _connectedClients;
        private static int ClientId { get; set; } = 0;

        private static void ThreadProc(object clientData)
        {
            var clientObject = (Client) clientData;
            Console.WriteLine($"Log: The client with id {clientObject.Id} has connected");
            while (true)
            {
                if (!clientObject.IsConnected())
                {
                    Console.WriteLine($"Log: Client with id {clientObject.Id} has disconnected");
                    _connectedClients.Remove(clientObject);
                    Thread.CurrentThread.Abort();
                }
                Thread.Sleep(5000);
            }
        }

        private static void StartListening()
        {
            TcpListener listener = null;
            _connectedClients = new List<Client>();
            try
            {
                // Set the listened port to 13000
                const int port = 13000;
                // TcpListener server = new TcpListener(port);
                listener = new TcpListener(IPAddress.Any, port);
                // Start listening for client requests.
                listener.Start();
                // Create a thread for all new client
                while (true)
                {
                    ClientId++;
                    var client = new Client(ClientId, listener.AcceptTcpClient());
                    _connectedClients.Add(client);
                    ThreadPool.QueueUserWorkItem(ThreadProc, client);
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                listener?.Stop();
            }
        }

        public static void Main()
        {
            var socketThread = new Thread(StartListening);
            socketThread.Start();
            Console.WriteLine("\nHit enter to refresh the clients");
            while (true)
            {
                Console.Read();
                Console.Clear();
                foreach (var client in _connectedClients)
                {
                    Console.WriteLine(client.Reference.Client.RemoteEndPoint);
                }
            }
        }
    }

    public class Client
    {
        public int Id { get; }
        public TcpClient Reference { get; }


        public Client(int id, TcpClient reference)
        {
            this.Id = id;
            this.Reference = reference;
        }

        public bool IsConnected()
        {
            if (!Reference.Connected) return true;
            if (!Reference.Client.Poll(10, SelectMode.SelectRead) || Reference.Available != 0) return true;
            return false;
        }
    }
}