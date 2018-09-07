using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Utils.ConnectionManager
{
	/// <summary>
	/// Wrapper used to transmit Commands and Data by Socket
	/// </summary>
	[Serializable]
	public class Packet
	{
		
		public readonly Command Command;
		private readonly string[] Args;
		public readonly string Data;

		/// <summary>
		/// Constructor used to create a new Packet from Command, Args and Data
		/// </summary>
		/// <param name="command">Command to be transmitted</param>
		/// <param name="args">Args to be transmitted</param>
		/// <param name="data">Data to be transmitted</param>
		public Packet(Command command, string[] args, string data)
		{
			Command = command;
			Args = args;
			Data = data;
		}

		/// <summary>
		/// Method used to serialize a Packet to an array of byte
		/// </summary>
		/// <returns>Packet converted to an array of byte</returns>
		public byte[] Serialize()
		{
			var formatter = new BinaryFormatter();
			using (var stream = new MemoryStream())
			{
				formatter.Serialize(stream, this);
				var data = stream.ToArray();
				var endSequence = Encoding.ASCII.GetBytes("ENDSEQ");
				var full = new byte[data.Length + endSequence.Length];
				data.CopyTo(full, 0);
				endSequence.CopyTo(full, data.Length);
				return full;
			}
		}

		/// <summary>
		/// Method used to deserialize a Packet from a array of byte
		/// </summary>
		/// <param name="arrBytes">Array of byte representing a Packet</param>
		/// <returns>Deserialized Packet</returns>
		public static Packet Deserialize(byte[] arrBytes)
		{
			using (var stream = new MemoryStream())
			{
				var formatter = new BinaryFormatter();
				stream.Write(arrBytes, 0, arrBytes.Length);
				stream.Seek(0, SeekOrigin.Begin);
				var obj = formatter.Deserialize(stream);
				return (Packet) obj;
			}
		}
	}
}
