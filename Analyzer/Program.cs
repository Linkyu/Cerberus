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
                var dict = new Dictionary<char, int>();
                var i = 0;
                var unknowns = 0;
                var dashes = 0;
                
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (line == null) continue;
                    
                    var values = line.Split('\t');

                    var key = values[3].ToUpper();
                    if (key.Length == 2)
                    {
                        foreach (var c in key)
                        {
                            if (!new List<char> {'A', 'T', 'C', 'G'}.Contains(c)) continue;
                            
                            if (dict.ContainsKey(c))
                            {
                                dict[c]++;
                            }
                            else
                            {
                                dict[c] = 1;
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
                }

                Console.WriteLine();
                Console.WriteLine(i + " pairs read");

                foreach (var pair in dict)
                {
                    Console.WriteLine(pair.Key + ": " + pair.Value + " - " + (float)pair.Value / i * 50 + "%");
                }

                Console.WriteLine("--: " + unknowns + " - " + (float)dashes / i * 50 + "%");
                Console.WriteLine("??: " + unknowns + " - " + (float)unknowns / i * 50 + "%");
            }
            
            stopWatch.Stop();
            Console.WriteLine("Time elapsed:" + stopWatch.ElapsedMilliseconds + "ms");
        }
    }
}