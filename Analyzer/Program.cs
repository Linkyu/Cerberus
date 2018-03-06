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
            
            const string genomeFile = @"..\..\resources\genome-manusporny.txt";
            
            using(var reader = new StreamReader(genomeFile))
            {
                const char separator = '\t';
                var genotypesDictionary = new Dictionary<char, int>();
                var i = 0;
                var unknowns = 0;
                var dashes = 0;
                
                
                reader.ReadLine();  // Skip the first line (it's the header)
                
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (line == null) continue;
                    
                    var values = line.Split(separator);

                    var key = values[3].ToUpper();
                    if (key.Length == 2)    // As of now, we don't parse single-letter genotypes since we don't know how to process them
                    {
                        foreach (var c in key)
                        {
                            if (!new List<char> {'A', 'T', 'C', 'G'}.Contains(c)) continue;    // Ignore indel varients and "--"
                            
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
                    //Console.WriteLine(i);    // Display current read line (slow!!!)
                }

                Console.WriteLine();
                Console.WriteLine(i + " pairs read");

                foreach (var pair in genotypesDictionary)
                {
                    Console.WriteLine(pair.Key + ": " + pair.Value + " - " + (float)pair.Value / i * 50 + "%");
                }

                Console.WriteLine("--: " + dashes + " - " + (float)dashes / i * 50 + "%");
                Console.WriteLine("??: " + unknowns + " - " + (float)unknowns / i * 50 + "%");
                
                
                // Sort the file by position
                var lines = File.ReadAllLines(genomeFile).ToList();
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