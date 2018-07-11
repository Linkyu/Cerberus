using System;
using System.Collections.Generic;
using System.IO;

namespace Worker
{
    public class Orchestrator
    {
        private ConnectionManager _connectionManager;
        public string Ip { get; }
        private FileInfo file;
        private Dictionary<string, Chunk> tasks;

        private const int MaxTaskPerCalculator = 2;

        public Orchestrator(string orchestratorIp)
        {
            Ip = orchestratorIp;
        }

        public void ReceiveConnection(Connection connection)
        {
            if (connection.type == "client")
            {
                AcceptClient(connection.ip);
            }
            else
            {
                AcceptCalculator(connection.ip);
            }
        }

        private void AcceptClient(string clientIp)
        {
            _connectionManager = new ConnectionManager(clientIp);
        }

        private void AcceptCalculator(string calculatorIp)
        {
            _connectionManager.AddCalculator(calculatorIp);
        }

        // should return a stat blob, not "object"
        // should also receive which module to execute
        public object ReceiveFile(FileInfo file)
        {
            this.file = file;

            RunProcess();
            
            return new NotImplementedException("not finished");
        }

        private void RunProcess()
        {
            SplitFile();

            while (HasRemainingTasks())
            {
                Collect();
                
                Distribute();
            }
        }

        // the actual method that retrieves the result from the calculator
        private void Collect()
        {
            foreach (var chunk in tasks)
            {
                if (!chunk.Value.processed || chunk.Value.IsAssigned())
                {
                    //chunk.Value.taskee.Run();
                }
            }
        }

        // assign chunks to calculators
        private void Distribute()
        {
            foreach (var chunk in tasks)
            {
                if (chunk.Value.processed || chunk.Value.IsAssigned()) continue;
                
                foreach (var calculatorPair in _connectionManager.calculators)
                {
                    if (calculatorPair.Value.Jobs.Count >= MaxTaskPerCalculator) continue;
                    
                    chunk.Value.Assign(calculatorPair.Value);
                    chunk.Value.taskee.TakeJob(new Tuple<string, string>(chunk.Key, chunk.Value.part));
                }
            }
        }

        private bool HasRemainingTasks()
        {
            foreach (var chunk in tasks)
            {
                if (!chunk.Value.processed)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Split the file into identified chunks so they can be assigned to a calculator
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void SplitFile()
        {
            throw new NotImplementedException();
        }
    }
}