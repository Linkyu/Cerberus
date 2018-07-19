using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Module_3
{
    internal class Program
    {
        public static void Main()
        {
            
            var numChromosome = 2; //pour l'instant, on ne focalise la recherche que sur un chromosome;
            const string genomeFile = @"..\..\elgrian.txt";
            List<string>occurences = new List<string>();  // la liste des bases azotées pour un chromosome
            using (var reader = new StreamReader(genomeFile)) {

                reader.ReadLine();
                
                while (!reader.EndOfStream){
                    var fileLine = reader.ReadLine();
                    var tokens = fileLine.Split('\t');

                    int chromosome = -1;
                    
                    if ( int.TryParse(tokens[1], out chromosome) &&  chromosome == numChromosome )
                    {
                        
                        occurences.Add(fileLine); //toutes les occurences du fichier pour un chromosome donné
                    }
                }
            }
            //Rechercher un gène dans un chromosome donné
            var basesList = new List<string>();
            StringBuilder builder = new StringBuilder();
            foreach (var occurence in occurences) {
                builder = new StringBuilder();
                var tokens = occurence.Split('\t');
                char firstBase = tokens[3][0];
                if (firstBase == '-' || firstBase == ' ' || firstBase == 'I' || firstBase == 'D' || firstBase == 'T') {
                } else {
                  //ajouter les couples à la liste des bases
                  basesList.Add(tokens[3]);
                }
            }

            foreach (var baseCouple in basesList)
            {
                builder.Append(baseCouple[0]);
            }

            //int nbCodonsMax = 129;
            
            //Premiere phase : à partir de la première base azotée.
            string firstDnaStrand = builder.ToString();
            
            
            //builder = new StringBuilder();

            /*for (var position = 0; position < 3 * nbCodonsMax; position++)
            {
                builder.Append(FirstDnaStrand[position]);
            }*/

            //FirstDnaStrand = builder.ToString();
            Console.WriteLine(firstDnaStrand);
            //Chercher le premier codon stop*
            var firstStop = firstDnaStrand.IndexOf("TAA", new StringComparison());
            if (firstStop == -1) {
                Console.WriteLine("Aucun codon STOP trouvé");
            }

            byte[] str = new byte[15];
            builder = new StringBuilder(49511);
            Console.WriteLine(firstDnaStrand.Length + " nucleotides presents dans la sequence sur " + builder.Capacity + " nucleotides possibles.");
            
        }
    }
}