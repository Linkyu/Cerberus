using System;

namespace Modules
{
	internal static class Program
	{
		public static void Main()
		{
			Console.WriteLine("1 - Analyzer");
			Console.WriteLine("2 - Finder");
			Console.WriteLine("3 - Search 1");
			var choice = Console.ReadLine();
			switch (int.Parse(choice))
			{
				case 1:
					Analyzer.MainAnalyzer();
					break;
				case 2:
				{
					Console.WriteLine("Pattern? >");
					var pattern = Console.ReadLine();
					Console.WriteLine("Limit? >");
					var limit = Console.ReadLine();

					Finder.MainFinder(pattern, int.Parse(limit));
					break;
				}
				case 3:
					break;
			}
		}
	}
}
