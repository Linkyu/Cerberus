using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Utils
{
	[Serializable]
	public class Packet
	{
		private Commands Command;
		private string[] Args;
		private string Data;

		public Packet(Commands command, string[] args, string data)
		{
			Command = command;
			Args = args;
			Data = data;
		}

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
