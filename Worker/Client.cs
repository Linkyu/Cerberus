using System;
using System.IO;

namespace Worker
{
    public class Client
    {
        private string ip;
        
        public Client(string clientIp)
        {
            ip = clientIp;
        }

        public Connection Connect(string clientIp)
        {
            return new Connection(clientIp, "client");
        }

        public FileInfo UploadFile()
        {
            return new FileInfo(@"..\..\resources\genome-manusporny.txt");
        }
    }
}