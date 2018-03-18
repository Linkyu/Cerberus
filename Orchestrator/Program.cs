using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Orchestrator
{
  internal static class Orchestartor
  {
    private static List<TcpClient> _listConnectedClients;
    private static int ClientNbr { get; set; } = 0;

    private static void ThreadProc(object clientData)
    {
      var client = (TcpClient) clientData;//.clientRef;
      Console.WriteLine("Log: A new client has connected");// " + clientData.nbr);
      while(true)
      {
        if(client.Connected)
        {
          if(client.Client.Poll(10,SelectMode.SelectRead) && client.Available==0)
          {
            Console.WriteLine("Log: A client has disconnected");// " + clientData.nbr);
            Thread.CurrentThread.Abort();
          }
        }
        Thread.Sleep(5000);
      }
    }
    private static void StartListening(){
      TcpListener listener=null;
      _listConnectedClients = new List<TcpClient>();
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
          ClientNbr++;
          var clientData = new { nbr = ClientNbr, clientRef = listener.AcceptTcpClient() }; 
          _listConnectedClients.Add(clientData.clientRef);
          ThreadPool.QueueUserWorkItem(ThreadProc, clientData.clientRef);
        }
      }
      catch(SocketException e)
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
      var socketThread = new Thread (Orchestartor.StartListening);
      socketThread.Start();
      Console.WriteLine("\nHit enter to refresh the clients");
      while (true)
      {
        Console.Read();
        Console.Clear();
        foreach (var client in _listConnectedClients) {
          Console.WriteLine(client.Client.LocalEndPoint.ToString() + " " + client.Connected + "1");
        }
      }
    }
  }
}