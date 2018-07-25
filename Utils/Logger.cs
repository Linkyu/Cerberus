using System;

namespace Utils
{
	public static class Logger
	{
		public static void Error(string message)
		{
			Console.WriteLine($"ERROR : {message}");
		}
		
		public static void Warning(string message)
		{
			Console.WriteLine($"WARNING : {message}");
		}

		public static void Debug(string message)
		{
			Console.WriteLine($"DEBUG : {message}");
		}
	}
}
