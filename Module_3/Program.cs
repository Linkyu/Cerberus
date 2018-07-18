using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace Module_3
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            
            Dictionary<string, string> geneticCode = new Dictionary<string, string>();
            geneticCode["UUU"] = "Phenilalaline";
            geneticCode["UUC"] = "Phenilalanine";
            geneticCode["UUA"] = "Leucine";
            geneticCode["UUG"] = "Leucine";
            geneticCode["CUU"] = "Leucine";
            geneticCode["CUC"] = "Leucine";
            geneticCode["CUA"] = "Leucine";
            geneticCode["CUG"] = "Leucine";
            geneticCode["AUU"] = "Isoleucine";
            geneticCode["AUC"] = "Isoleucine";
            geneticCode["AUA"] = "Isoleucine";
            geneticCode["AUG"] = "Methionine";
            geneticCode["GUU"] = "Valine";
            geneticCode["GUC"] = "Valine";
            geneticCode["GUA"] = "Valine";
            geneticCode["GUG"] = "Valine";
            geneticCode["UCU"] = "Serine";
            geneticCode["UCC"] = "Serine";
            geneticCode["UCA"] = "Serine";
            geneticCode["UCG"] = "Serine";
            geneticCode["CCU"] = "Proline";
            geneticCode["CCC"] = "Proline";
            geneticCode["CCA"] = "Proline";
            geneticCode["CCG"] = "Proline";
            geneticCode["ACU"] = "Threonine";
            geneticCode["ACC"] = "Threonine";
            geneticCode["ACA"] = "Threonine";
            geneticCode["ACG"] = "Theonine";
            geneticCode["GCU"] = "Alanine";
            geneticCode["GCC"] = "Alanine";
            geneticCode["GCA"] = "Alanine";
            geneticCode["GCG"] = "Alanine";
            geneticCode["UAU"] = "Thyrosine";
            geneticCode["UAC"] = "Thyrosine";
            geneticCode["UAA"] = "STOP";
            geneticCode["UAG"] = "STOP";
            geneticCode["CAU"] = "Histidine";
            geneticCode["CAC"] = "Histidine";
            geneticCode["CAA"] = "Glutamine";
            geneticCode["CAG"] = "Glutamine";
            geneticCode["AAU"] = "Asparagine";
            geneticCode["AAC"] = "Asparagine";
            geneticCode["AAA"] = "Lysine";
            geneticCode["AAG"] = "Lysine";
            geneticCode["GAU"] = "Acide aspartique";
            geneticCode["GAC"] = "Acide aspartique";
            geneticCode["GAA"] = "Acide glutamique";
            geneticCode["GAG"] = "Acide glutamique";
            geneticCode["UGU"] = "Cysteine";
            geneticCode["UGC"] = "Cysteine";
            geneticCode["UGA"] = "STOP";
            geneticCode["UGG"] = "Tryptophane";
            geneticCode["CGU"] = "Arginine";
            geneticCode["CGC"] = "Arginine";
            geneticCode["CGA"] = "Arginine";
            geneticCode["CGG"] = "Arginine";
            geneticCode["AGU"] = "Serine";
            geneticCode["AGC"] = "Serine";
            geneticCode["AGA"] = "Arginine";
            geneticCode["AGG"] = "Arginine";
            geneticCode["GCU"] = "Glycine";
            geneticCode["GGC"] = "Glycine";
            geneticCode["GGA"] = "Glycine";
            geneticCode["GGG"] = "Glycine";

            Dictionary<char, char> NitrogenBaseAssociations = new Dictionary<char, char>();
            NitrogenBaseAssociations['A'] = 'T';
            NitrogenBaseAssociations['T'] = 'A';
            NitrogenBaseAssociations['C'] = 'G';
            NitrogenBaseAssociations['G'] = 'C';

            int numChromosome = 8; //pour l'instant, on ne focalise la recherche que sur un chromosome;
            const string genomeFile = @"..\..\elgrian.txt";
            List<string>occurences = new List<string>();  // la liste des bases azotées pour un chromosome
            using (var reader = new StreamReader(genomeFile)) {

                reader.ReadLine();
                
                while (!reader.EndOfStream){
                    var FileLine = reader.ReadLine();
                    var tokens = FileLine.Split('\t');

                    int chromosome = -1;
                    
                    if ( int.TryParse(tokens[1], out chromosome) &&  chromosome == numChromosome )
                    {
                        
                        occurences.Add(FileLine); //toutes les occurences du fichier pour un chromosome donné
                    }
                }
            }
            //Rechercher un gène dans un chromosome donné
            var BasesList = new List<string>();
            StringBuilder builder = new StringBuilder();
            int index = 0;
            foreach (var occurence in occurences) {
                builder = new StringBuilder();
                var tokens = occurence.Split('\t');
                char FirstBase = tokens[3][0];
                if (FirstBase == '-' || FirstBase == ' ' || FirstBase == 'I' || FirstBase == 'D' || FirstBase == 'T') {
                    continue;
                } else {
                  //ajouter les couples à la liste des bases
                  BasesList.Add(tokens[3]);
                }
                index++;
            }

            foreach (var BaseCouple in BasesList)
            {
                builder.Append(BaseCouple[0]);
            }

            //int nbCodonsMax = 129;
            
            //Premiere phase : à partir de la première base azotée.
            string FirstDnaStrand = builder.ToString();
            
            //builder = new StringBuilder();

            /*for (var position = 0; position < 3 * nbCodonsMax; position++)
            {
                builder.Append(FirstDnaStrand[position]);
            }*/

            //FirstDnaStrand = builder.ToString();
            Console.WriteLine(FirstDnaStrand);
            //Chercher le premier codon stop*
            var FirstStop = FirstDnaStrand.IndexOf("TAA");
            if (FirstStop == -1) {
                Console.WriteLine("Aucun codon STOP trouvé");
            }
            
            
        }
    }
}