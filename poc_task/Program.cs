using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace poc_task
{

    class Program {

        private static readonly string[] InputFiles =
        {
            @"..\..\genome-greshake.txt", @"..\..\genome-kennethreitz.txt", @"..\..\genome-kukushkin.txt",
            @"..\..\genome-manusporny.txt", @"..\..\genome-quartzjer.txt", @"..\..\genome-soffes.txt"
        };


        private static List<string> _dnaInputFiles;

        private static void Main()
        {

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            _dnaInputFiles = new List<string>();
            foreach (var inputFile in InputFiles) {
                _dnaInputFiles.Add(inputFile);
            }

            var results = _dnaInputFiles.Select((t, i) => Task.Run(() => ParseGenome(_dnaInputFiles.ElementAt(i)))).Select(infos => infos.Result).ToList();

            foreach (var result in results)
            {
                if (result == null) continue;
                
                Console.WriteLine(result.Dashes + " dashes in this sequence.");
                Console.WriteLine(result.Unknowns + "unknowns pairs");
                Console.WriteLine("Adenine has " + result.InfosNitrogenBases['A'] +
                                  " occurences in this sequence. ");
                Console.WriteLine("Cytosine has " + result.InfosNitrogenBases['C'] +
                                  " occurences in this sequence. ");
                Console.WriteLine("Thymine has " + result.InfosNitrogenBases['T'] +
                                  " occurences in this sequence. ");
                Console.WriteLine("Guanine has " + result.InfosNitrogenBases['G'] +
                                  " occurences in this sequence. ");
            }
            
            stopwatch.Stop();
            Console.Out.WriteLine("Time elapsed : " + stopwatch.ElapsedMilliseconds + " ms.");
        }

        private static DNAStatisticsInfos ParseGenome(string fileName) {
            /**********************************************************************************************/
            var infos = new DNAStatisticsInfos();
            using (var reader = new StreamReader(fileName)) {
                const char separator = '\t';
                var genotypesDictionary = new Dictionary<char, int>();
                
                reader.ReadLine(); // Skip the first line (it's the header)

                while (!reader.EndOfStream) 
                {
                    var line = reader.ReadLine();
                    if (line == null) continue;

                    var values = line.Split(separator);

                    var key = values[3].ToUpper();
                    if (key.Length == 2
                    ) // As of now, we don't parse single-letter genotypes since we don't know how to process them
                    {
                        foreach (var c in key) {
                            if (!new List<char> {'A', 'T', 'C', 'G'}.Contains(c))
                                continue; // Ignore indel varients and "--"

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
                            infos.Dashes++;
                        }
                    }
                    else
                    {
                        infos.Unknowns++;
                    }
                }

                // Sort the file by position
                var lines = File.ReadAllLines(fileName).ToList();
                lines.RemoveAt(0);
                var orderedLines = lines.OrderBy(s => int.Parse(s.Split(separator)[2])).ToList();
                var sequencesDictionary = new Dictionary<string, int>();

                for (var j = 0; j < orderedLines.Count - 1; j++)
                {
                    var key = orderedLines[j].Split(separator)[3] + orderedLines[j + 1].Split(separator)[3];
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
                infos.InfosNitrogenBases = genotypesDictionary;
            }
            /***********************************************************************************************/

            return infos;
        }

    }

    // ReSharper disable once InconsistentNaming
    public class DNAStatisticsInfos
    {
        public int Dashes { get; set; }
        public int Unknowns { get; set; }
        public Dictionary<char, int> InfosNitrogenBases { get; set; }

        private const int DefaultValue = 0;

        public DNAStatisticsInfos()
        {
            Dashes = DefaultValue;
            Unknowns = DefaultValue;
            InfosNitrogenBases = new Dictionary<char, int>();

        }
    }
}