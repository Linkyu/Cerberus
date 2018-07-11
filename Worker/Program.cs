namespace Worker
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            /* PLAN:
             * 
             * create all parts
             * connect each parts
             * upload file to orchestrator
             * split file
             * send to calculators with LM
             * parse each part
             * compile parts
             * make stats
             * display stats
             */
            
            var client = new Client("192.168.1.10");
            var orchestrator = new Orchestrator("129.168.1.30");
            var calculator1 = new Calculator("192.168.1.20");
            var calculator2 = new Calculator("192.168.1.21");
            var calculator3 = new Calculator("192.168.1.22");
            
            orchestrator.ReceiveConnection(calculator1.Connect(orchestrator.Ip));
            orchestrator.ReceiveConnection(calculator2.Connect(orchestrator.Ip));
            orchestrator.ReceiveConnection(calculator3.Connect(orchestrator.Ip));
            orchestrator.ReceiveConnection(client.Connect(orchestrator.Ip));

            var stats = orchestrator.ReceiveFile(client.UploadFile());
            
        }
    }
}