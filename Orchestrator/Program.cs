using Utils;
using Utils.ConnectionManager;

namespace Orchestrator
{
    internal static class Program
    {
	    private static void ProcessPacket(Packet packet, SocketContainer socketContainer)
	    {
		    Logger.Debug($"Orchestrator - ProcessPacket -> {packet.Command} - {packet.Data}");
		    // TODO => Implement next lines
		    // Get the correct worker (packet.Command)
		    // Add a job to this worker
		    // If response
		    // Create a new packet
		    // Send the response packet
	    }
	    
        public static void Main(string[] args)
        {
	        // TODO => Move to Orchestrator constructor 
	        var connector = new ServerConnector("127.0.0.1", 3003);
	        connector.PacketReceived += ProcessPacket;
	        connector.Listen();
	        while (true) {}
        }
    }
}