using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Analyzer
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            using(var reader = new StreamReader(@"..\..\resources\genome-manusporny.txt"))
            {
                const char separator = '\t';
                var genotypesDictionary = new Dictionary<char, int>();
                var i = 0;
                var unknowns = 0;
                var dashes = 0;
                
                
                reader.ReadLine();  // Skip the first line (header)
                
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (line == null) continue;
                    
                    var values = line.Split(separator);

                    //int.TryParse(values[2], out var pos);
                    //orderedGenome.Insert(SearchIndex(orderedGenome, pos), line);
                    //var index = orderedGenome.BinarySearch(pos);

                    var key = values[3].ToUpper();
                    if (key.Length == 2)
                    {
                        foreach (var c in key)
                        {
                            if (!new List<char> {'A', 'T', 'C', 'G'}.Contains(c)) continue;
                            
                            if (genotypesDictionary.ContainsKey(c))
                            {
                                genotypesDictionary[c]++;
                            }
                            else
                            {
                                genotypesDictionary[c] = 1;
                            }
                        }

                        if (key == "--")
                        {
                            dashes++;
                        }
                    }
                    else
                    {
                        unknowns++;
                    }

                    i++;
                    //Console.WriteLine(i);
                }

                Console.WriteLine();
                Console.WriteLine(i + " pairs read");

                foreach (var pair in genotypesDictionary)
                {
                    Console.WriteLine(pair.Key + ": " + pair.Value + " - " + (float)pair.Value / i * 50 + "%");
                }

                Console.WriteLine("--: " + dashes + " - " + (float)dashes / i * 50 + "%");
                Console.WriteLine("??: " + unknowns + " - " + (float)unknowns / i * 50 + "%");

                var lines = File.ReadAllLines(@"..\..\resources\genome-manusporny.txt").ToList();
                lines.RemoveAt(0);
                var orderedLines = lines.OrderBy(s => int.Parse(s.Split(separator)[2])).ToList();
                var sequencesDictionary = new Dictionary<string, int>();
                
                for (var j = 0; j < orderedLines.Count - 1; j++)
                {
                    var key = orderedLines[j].Split(separator)[3] + orderedLines[j+1].Split(separator)[3];
                    if (key.Length != 4 || key.Contains('D') || key.Contains('I') || key.Contains('-')) continue;
                    
                    if (sequencesDictionary.ContainsKey(key))
                    {
                        sequencesDictionary[key]++;
                    }
                    else
                    {
                        sequencesDictionary[key] = 1;
                    }
                }
                
                var sortedSequencesDictionary = from entry in sequencesDictionary orderby entry.Value descending select entry;

                Console.WriteLine();
                Console.WriteLine("4-sequences occurences:");
                foreach (var pair in sortedSequencesDictionary)
                {
                    Console.WriteLine(pair.Key + ": " + pair.Value);
                    //break; // We only want the highest one
                }
            }
            
            stopWatch.Stop();
            Console.WriteLine("Time elapsed:" + stopWatch.ElapsedMilliseconds + "ms");
        }
    }
}