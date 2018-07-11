using System;
using System.Collections.Generic;

namespace Worker
{
    public class ConnectionManager
    {
        private Client client;
        internal Dictionary<string, Calculator> calculators;

        public ConnectionManager(string clientIp)
        {
            client = new Client(clientIp);
            calculators = new Dictionary<string, Calculator>();
        }

        public void AddCalculator(string calculatorIp)
        {
            calculators[GenerateName()] = new Calculator(calculatorIp);
        }

        private static string GenerateName()
        {
            throw new System.NotImplementedException();
        }
    }

    internal class Chunk
    {
        internal Calculator taskee;
        internal string part;
        internal bool processed = false;

        public Chunk(string part)
        {
            this.part = part;
        }

        internal void Assign(Calculator calculator)
        {
            taskee = calculator;
        }

        internal void Free()
        {
            if (processed)
            {
                throw new Exception("Can't free a finished task!");
            }

            taskee = null;
        }

        public bool IsAssigned()
        {
            return taskee != null;
        }
    }

    public class Connection
    {
        internal string ip;
        internal string type;

        public Connection(string ip, string type)
        {
            this.ip = ip;
            this.type = type;
        }
    }
}