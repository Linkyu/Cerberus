using System;
using System.Collections.Generic;

namespace Worker
{
    public class Calculator
    {
        private string ip;
        public Dictionary<string, string> Jobs { get; }

        public Calculator(string calculatorIp)
        {
            ip = calculatorIp;
        }

        public Connection Connect(string clientIp)
        {
            return new Connection(clientIp, "calculator");
        }

        // item1 = task id, item 2 = task
        public void TakeJob(Tuple<string, string> part)
        {
            Jobs[part.Item1] = part.Item2;
        }
    }
}