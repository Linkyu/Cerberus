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
            var numChromosome = 20; //pour l'instant, on ne focalise la recherche que sur un chromosome;
            const string genomeFile = @"..\..\elgrian.txt";
            List <string>occurences = new List<string>();  // la liste des bases azotées pour un chromosome
            try
            {
                using (var reader = new StreamReader(genomeFile))
                {
                    reader.ReadLine();
                    while (!reader.EndOfStream)
                    {
                        var fileLine = reader.ReadLine();
                        if (!fileLine.StartsWith("#"))
                        {

                            var tokens = fileLine.Split('\t');

                            var chromosome = -1;

                            if (int.TryParse(tokens[1], out chromosome) && chromosome == numChromosome)
                            {
                                occurences.Add(fileLine); //toutes les occurences du fichier pour un chromosome donné
                            }
                        }
                    }
                }
            }
            catch (FileNotFoundException err) {
                 Console.WriteLine(genomeFile + " introuvable.");
            } 

            //Rechercher un gène dans un chromosome donné
            var basesList = new List<string>();
            StringBuilder builder = new StringBuilder();
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
            
            //Premiere phase : à partir de la première base azotée.
            var firstDnaStrand = builder.ToString();
            
            
            //Chercher le premier codon stop*
            var firstStop = firstDnaStrand.IndexOf("TAA", new StringComparison());
            Console.WriteLine(firstStop == -1 ? "Aucun codon STOP trouvé" : "Codon stop trouvé à la ligne : " + firstStop);
            Console.WriteLine(firstDnaStrand.Length + " nucleotides presents dans la sequence sur " + builder.Capacity + " nucleotides possibles.");
            
            //Chercher le deuxieme codon stop
            var secondStop = firstDnaStrand.IndexOf("TAA", firstStop + 3, new StringComparison());
            Console.WriteLine(secondStop == -1 ? "Aucun codon STOP trouvé" : "Codon stop trouvé à la ligne : " + secondStop);
            var initiationIndex = -1;
                
            if (secondStop - firstStop - 3 > 0)
            {
                //Chercher un codon start
                initiationIndex = firstDnaStrand.IndexOf("ATG", firstStop + 3, new StringComparison());
                if (initiationIndex > -1)
                {
                    Console.WriteLine("Un codon start a été trouvé à l'index " + initiationIndex);
                }
            }

            if (initiationIndex < secondStop && initiationIndex > firstStop)
            {
                //le start est situé entre deux codon stop
                Console.WriteLine(
                    initiationIndex > -1 && ((secondStop + 3) - initiationIndex) > 0 &&
                    ((secondStop + 3) - initiationIndex) % 3 == 0
                        ? "Une séquence codante a peut être été trouvé."
                        : (secondStop + 3) - initiationIndex +
                          " est négatif ou n'est pas un multiple de 3. La séquence est non codante.");
            }

            builder = new StringBuilder();
            //Affichage de la séquence codante.
            if (((secondStop + 3) - initiationIndex) % 3 == 0)
            {
                for (var i = initiationIndex; i < secondStop + 3; i++)
                {
                    builder.Append(firstDnaStrand[i]);
                }

                Console.WriteLine(builder.ToString());
            }
        }
    }
}