using System;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;

using System.Linq;


namespace ConsoleApp1
{

    class Program {

        private static string[] INPUT_FILES =
        {
            @"..\..\genome-greshake.txt", @"..\..\genome-kennethreitz.txt", @"..\..\genome-kukushkin.txt",
            @"..\..\genome-manusporny.txt", @"..\..\genome-quartzjer.txt", @"..\..\genome-soffes.txt"
        };


        private static List<string> dna_input_files = null;

        static void Main() {

            dna_input_files = new List<string>();
            foreach (var inputFile in INPUT_FILES) {
                dna_input_files.Add(inputFile);
            }

            for (var i = 0; i < dna_input_files.Count; i++) {
                Task<DNAStatisticsInfos> infos = Task.Run(() => parseGenome(dna_input_files.ElementAt(i)));
                Task.WaitAll();
                DNAStatisticsInfos statistics = infos.Result;
                if (statistics != null) {
                    dna_input_files.RemoveAt(i);
                    Console.WriteLine(statistics.Dashes + " dashes in this sequence.");
                    Console.WriteLine(statistics.Unknowns + "unknons pairs");
                    Console.WriteLine("Adenine has " + statistics.infosNitrogenBases['A'] +
                                      " occurences in this sequence. ");
                    Console.WriteLine("Cytosine has " + statistics.infosNitrogenBases['C'] +
                                      " occurences in this sequence. ");
                    Console.WriteLine("Thymine has " + statistics.infosNitrogenBases['T'] +
                                      " occurences in this sequence. ");
                    Console.WriteLine("Guanine has " + statistics.infosNitrogenBases['G'] +
                                      " occurences in this sequence. ");
                }

            }
        }

        public static DNAStatisticsInfos parseGenome(string fileName) {
            /**********************************************************************************************/
            DNAStatisticsInfos infos = new DNAStatisticsInfos();
            using (var reader = new StreamReader(fileName)) {
                const char separator = '\t';
                var genotypesDictionary = new Dictionary<char, int>();
                var i = 0;
                var unknowns = 0;
                var dashes = 0;
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
                            //dashes++;
                            infos.Dashes++;
                        }
                    }
                    else
                    {
                        infos.Unknowns++;
                    }

                    i++;
                    //Console.WriteLine(i);    // Display current read line (slow!!!)
                }

                Console.WriteLine();
                Console.WriteLine(i + " pairs read");

                foreach (var pair in genotypesDictionary)
                {
                    Console.WriteLine(pair.Key + ": " + pair.Value + " - " + (float) pair.Value / i * 50 + "%");
                }

                Console.WriteLine("--: " + dashes + " - " + (float) dashes / i * 50 + "%");
                Console.WriteLine("??: " + unknowns + " - " + (float) unknowns / i * 50 + "%");


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

                var sortedSequencesDictionary =
                    from entry in sequencesDictionary orderby entry.Value descending select entry;
                infos.infosNitrogenBases = genotypesDictionary;

                Console.WriteLine();
                Console.WriteLine("4-sequences occurences:");
                foreach (var pair in sortedSequencesDictionary)
                {
                    Console.WriteLine(pair.Key + ": " + pair.Value);
                    //break; // We only want the highest one
                }
            }
            /***********************************************************************************************/

            return infos;
        }

    }

    public class DNAStatisticsInfos
    {
        public int Dashes { get; set; }
        public int Unknowns { get; set; }
        public Dictionary<char, int> infosNitrogenBases { get; set; }

        private static int DEFAULT_VALUE = 0;

        public DNAStatisticsInfos()
        {
            Dashes = DEFAULT_VALUE;
            Unknowns = DEFAULT_VALUE;
            infosNitrogenBases = new Dictionary<char, int>();

        }
    }
}