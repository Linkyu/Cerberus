using Utils;
using Utils.ConnectionManager;

namespace Client
{
    internal static class Program
    {
	    private static ClientConnector connector;
	    
	    private static void ProcessPacket(Packet packet, SocketContainer socketContainer)
	    {
		    Logger.Debug($"Client - ProcessPacket -> {packet.Command}");
		    // TODO => Remove next 3 lines, it's just for test purpose
		    if (packet.Command != Command.Identify) return;
		    var responsePacket = new Packet(Command.IdentifyAck,null,"Client");
		    BaseConnector.Send(responsePacket, socketContainer);
		    // TODO => Implement next lines
		    // Get the correct worker (packet.Command)
		    // Add a job to this worker
		    // If response
		    // Create a new packet
		    // Send the response packet
	    }
	    
	    public static void Main(string[] args)
	    {
		    // TODO => Move to Client constructor 
		    connector = new ClientConnector("127.0.0.1", 3005);
		    connector.PacketReceived += ProcessPacket;
		    connector.Connect("127.0.0.1", 3003);
		    while (true) {}
	    }
    }
}