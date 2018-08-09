using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Module_3;

namespace Module_3
{
    internal class Program
    {
        public static void Main()
        {
            /*const string genomeFile = @"..\..\karen.txt";
            var occurences = new List<string>();  // la liste des bases azotées pour un chromosome
            
            try
            {
                using (var reader = new StreamReader(genomeFile))
                {
                    reader.ReadLine();
                    while (!reader.EndOfStream)
                    {
                        var fileLine = reader.ReadLine();
                        if (fileLine != null && fileLine.StartsWith("#")) continue;

                        if (fileLine == null) continue;
                        var tokens = fileLine.Split('\t');
                        occurences.Add(fileLine); //toutes les occurences du fichier pour un chromosome donné
                    }
                }
            }
            catch (FileNotFoundException err) {
                 Console.WriteLine(genomeFile + " introuvable.");
            } 

            //Rechercher un gène dans un chromosome donné
            var basesList = new List<string>();
            var builder = new StringBuilder();
            foreach (var occurence in occurences) {
                builder = new StringBuilder();
                var tokens = occurence.Split('\t');
                if (tokens[3].Length == 2)
                {
                    var firstBase = tokens[3][1];
                    if (firstBase == '-' || firstBase == ' ' || firstBase == 'I' || firstBase == 'D')
                    {
                    }
                    else
                    {
                        //ajouter les couples à la liste des bases
                        basesList.Add(tokens[3]);
                    }
                }

            }

            foreach (var baseCouple in basesList)
            {
                builder.Append(baseCouple[0]);
            }*/
            
            //var firstDnaStrand = builder.ToString();
            var firstDnaStrand = "TAAATGGGAGAACGAAGCGATGTGCGTGCCTAGCGCTTGTATCCGCAAATAA";
            //Analyse de la séquence d'adn par la protéine qui va synthétiser un ARNMessager.
             RnaPolymerase polymerase = new RnaPolymerase(firstDnaStrand);
            
            Gene gene = polymerase.SearchGenomicSequence();
            Console.WriteLine(gene == null ? "Aucun gene trouve" : gene.GenomicSequence);

            if (gene == null)
            {
                return;
            }
            RiboNucleicAcid rna = polymerase.transcript(gene);
       
            
            Console.WriteLine(rna == null ? "Echec de la transcription" : rna.Sequence);
            
            Ribosom ribosom = new Ribosom();
            Protein finalProtein = ribosom.translate(rna, gene.indexStart);
            
            Console.WriteLine(finalProtein.ToString());
            
        }
    }
   



}