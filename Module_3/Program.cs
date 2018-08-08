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
            const string genomeFile = @"..\..\mimbee.txt";
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
                var  firstBase = tokens[3][0];
                if (firstBase == '-' || firstBase == ' ' || firstBase == 'I' || firstBase == 'D') {
                } else {
                    //ajouter les couples à la liste des bases
                    basesList.Add(tokens[3]);
                }
            }

            foreach (var baseCouple in basesList)
            {
                builder.Append(baseCouple[0]);
            }
            
            var firstDnaStrand = builder.ToString();
            
            //Analyse de la séquence d'adn par la protéine qui va synthétiser un ARNMessager.
             RNAPolymerase polymerase = new RNAPolymerase(firstDnaStrand);
            Gene gene = polymerase.SearchGenomicSequence();
            RiboNucleicAcid rna = polymerase.transcript(gene); 
            
            Console.WriteLine(gene == null ? "Aucun gene trouve" : gene.GenomicSequence);
            Console.WriteLine(rna == null ? "Echec de la transcription" : rna.Sequence);
        }
    }
   



}