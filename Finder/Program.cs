using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Finder
{
	internal static class Program
	{
		public static void Main()
		{
			var genomeMatchs = new List<GenomeMatch>();
			var matchPositions = new List<int>();
			// Start StopWatch to calculate process time
			var stopWatch = new Stopwatch();
			stopWatch.Start();
			
			
			// Check if command arguments are valid, exit program if it's not the case
			string matchArg;
			var topMatch = 0;
			try
			{
				matchArg = Environment.GetCommandLineArgs()[1].ToUpper();
				try
				{
					topMatch = Convert.ToInt32(Environment.GetCommandLineArgs()[2]);
					if (topMatch < 1)
					{
						Console.WriteLine("Wrong command argument, second argument must be a strictly positive int.");
						Environment.Exit(1);
					}
				}
				catch (FormatException)
				{
					Console.WriteLine("Wrong command argument, second argument must be a strictly positive int.");
					Environment.Exit(1);
				}
				
				const string argPattern = @"[^ATGC]";
				var argRgx = new Regex(argPattern);
				if (argRgx.Matches(matchArg).Count > 0)
				{
					Console.WriteLine("Wrong command argument, first argument must be a string containing only [A,C,G,T].");
					Environment.Exit(1);
				}
			}
			catch (IndexOutOfRangeException)
			{
				Console.WriteLine("Wrong command argument, first argument must be a string containing only [A,C,G,T], second argument must be a strictly positive int.");
				matchArg = "";
				Environment.Exit(1);
			}
            
			// Read genome file and create a string with [A,T,C,G] values
			const string genomeFile = @"../../resources/genome-manusporny.txt";
            
			var genotypesString = new StringBuilder();
			using(var reader = new StreamReader(genomeFile))
			{
				const char separator = '\t';
                
				reader.ReadLine();  // Skip the first line (it's the header)
                
				while (!reader.EndOfStream)
				{
					var line = reader.ReadLine();
					if (line == null) continue;
                    
					var values = line.Split(separator);
					var key = values[3].ToUpper();
					
					if (key.Length != 2) continue;
					if (key != "--")
					{
						genotypesString.Append(key);
					}
				}
			}
			
			// Search param argument match in genomtypes
			Console.WriteLine("Search top {0} matchs for {1}", topMatch, matchArg);
			var pattern = $"{matchArg}";
			var rgx = new Regex(pattern);
			//Console.WriteLine("Default pattern : {0}", pattern);
			foreach (Match match in rgx.Matches(genotypesString.ToString()))
			{
				if (!matchPositions.Contains(match.Index))
				{
					genomeMatchs.Add(new GenomeMatch(match.Index, pattern.Length, pattern.Length, match.Value));
					matchPositions.Add(match.Index);
				}
				if (matchPositions.Count < topMatch) continue;
				break;
			}

			if (matchPositions.Count < topMatch)
			{
				for (var jocker = 1; jocker < matchArg.Length; jocker++)
				{
					for (var i = 0; i <= matchArg.Length - jocker; i++)
					{
						var jockerPattern = new StringBuilder(pattern);
						jockerPattern.Remove(i, jocker).Insert(i, string.Concat(Enumerable.Repeat(".", jocker)));
						//Console.WriteLine("Jocker pattern : {0}", jockerPattern.ToString());
						var jockerRgx = new Regex(jockerPattern.ToString());
						foreach (Match match in jockerRgx.Matches(genotypesString.ToString()))
						{
							//Console.WriteLine("Match {0} : {1} {2}", jockerPattern.ToString(), match.Value, match.Index);
							if (!matchPositions.Contains(match.Index))
							{
								genomeMatchs.Add(new GenomeMatch(match.Index, jockerPattern.Length - jocker, jockerPattern.Length, match.Value));
								matchPositions.Add(match.Index);
							}
							if (matchPositions.Count < topMatch) continue;
							break;
						}
						if (matchPositions.Count < topMatch) continue;
						break;
					}
				}
			}
			
			genomeMatchs.Sort(new GenomeMatchComparer());
			for (var index= 0; index < genomeMatchs.Count; index++)
			{
				Console.WriteLine($"Rank {index}, {genomeMatchs[index]}");
			}
			
			// Stop StopWatch and display elapsed time
			stopWatch.Stop();
			Console.WriteLine("Time elapsed:" + stopWatch.ElapsedMilliseconds + "ms");
		}

		internal class GenomeMatch
		{
			public readonly int Position;
			public readonly int Match;
			private readonly int _maxMatch;
			private readonly string _result;

			public GenomeMatch(int position, int match, int maxMatch, string result)
			{
				Position = position;
				Match = match;
				_maxMatch = maxMatch;
				_result = result;
			}
			
			public override string ToString()
			{
				return $"position {Position}, match {Match}/{_maxMatch}, result : {_result}";
			}
		}

		private class GenomeMatchComparer : IComparer<GenomeMatch>
		{
			public int Compare(GenomeMatch x, GenomeMatch y)
			{
				if (x == null || y == null) return 0;
				int result;
				if (x.Match < y.Match)
					result = 1;
				else if (x.Match > y.Match)
					result = -1;
				else
					result = x.Position.CompareTo(y.Position);
				return result;

			}
		}
	}
}
