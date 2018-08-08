using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace Module_3
{
    public class Protein
    {
        //private List<string> listAminoAcid;
    }

    /*
     * RNAPolymerase is the protein witch makes RiboNucleic Acid from DNA
     *
     */

    public class RNAPolymerase : Protein
    {
        public string dnaSequence { get; set; }

        public RNAPolymerase(string dnaSequence)
        {
            this.dnaSequence = dnaSequence;

        }
        

        private static string getRNANucleotid(string nucleotid)
        {
            Dictionary<string, string> coupleBase = new Dictionary<string, string>
            {
                ["A"] = "A",
                ["T"] = "U",
                ["C"] = "C",
                ["G"] = "G"
            };
            return coupleBase[nucleotid];
        } 

        /// <summary>
        /// Search a gene in the given DNA sequence
        /// </summary>
        /// <returns></returns>
        public Gene SearchGenomicSequence()
        {
            var firstDnaStrand = dnaSequence;
            StringBuilder builder;
            //**************************************************************************************************
            // PARTIE 1 : Recherche de deux condons STOP et d'un start.
            //Chercher le premier codon stop*
            var firstStop = firstDnaStrand.IndexOf("TAA", new StringComparison());
            Console.WriteLine(firstStop == -1
                ? "Aucun codon STOP trouvé"
                : "Codon stop trouvé à l'index : " + firstStop + "du brin d'ADN");

            //Chercher le deuxieme codon stop
            var secondStop = firstDnaStrand.IndexOf("TAA", firstStop + 3, new StringComparison());
            Console.WriteLine(secondStop == -1
                ? "Aucun codon STOP trouvé"
                : "Codon stop trouvé à l'index : " + secondStop + " du brin d'ADN");
            var initiationIndex = -1;

            //On vérifie que le premier stop est bien avant la deuxième
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
                //le start est situé entre deux codon stop. Vérifier qu'il s'agisse bien d'une séquence codante.
                Console.WriteLine(
                    initiationIndex > -1 && ((secondStop + 3) - initiationIndex) > 0 &&
                    ((secondStop + 3) - initiationIndex) % 3 == 0
                        ? "Une séquence codante a peut être été trouvé."
                        : (secondStop + 3) - initiationIndex +
                          " est négatif ou n'est pas un multiple de 3. La séquence est non codante.");
            }

            builder = new StringBuilder();
            //Affichage de la séquence codante.
            if ((secondStop + 3 - initiationIndex) % 3 != 0) return null;
            for (var i = initiationIndex; i < secondStop + 3; i++)
            {
                builder.Append(firstDnaStrand[i]);
            }

            var candidateSequence =
                builder.ToString(); // séquence qui contient les deux stop et un start entre les 2.
            // ***************************************************************************************************************************
            //PARTIE 2 : Recherche d'un site de fixation du ribosome.
            //A partir de maintenant on ne travaille plus que sur une séquence candidate, c'est à dire celle qui se situe entre les 2 stops.

            /*Motifs possible pour un RBS :
             * AGGAGG
             * AGGA
             * GAGG
             * GGAG
             */

            var listRbs = new List<string>();
            listRbs.Add("AGGAGG");
            listRbs.Add("AGGA");
            listRbs.Add("GAGG");
            listRbs.Add("GGAG");

            //Chercher un site de fixation pour le ribosome
            var rbsIndex = 0;
            var rbsResult = "";
            foreach (var ribosomBindingSite in listRbs)
            {
                rbsIndex = candidateSequence.IndexOf(ribosomBindingSite, new StringComparison());
                if (rbsIndex > -1)
                {
                    rbsResult = ribosomBindingSite;
                }
                else
                {
                }
            }


            Console.WriteLine(rbsIndex == -1
                ? "Aucun site de fixation du ribosome n'a été trouvé"
                : rbsIndex + " : un site de fixation a été trouvé.");
            if (rbsIndex > -1)
            {
                //l'index du codon d'initiation ne va porter que sur la séquence candidate.
                initiationIndex =
                    candidateSequence.IndexOf("ATG", rbsIndex + rbsResult.Length + 1, new StringComparison());
                Console.WriteLine(initiationIndex == -1
                    ? "Codon start introuvable"
                    : initiationIndex + " : Codon start trouvé.");

            }


            //TODO : avant de renvoyer la séquence génomique, bien s'assurer qu'un codon start est présent entre le site de fixation du ribosome et le dernier codon stop.
            //Chercher un start APRES le RBS  
            initiationIndex =
                candidateSequence.IndexOf("ATG", rbsIndex + rbsResult.Length - 1, new StringComparison());

            if (initiationIndex > -1)
            {
                //Un codon start a été trouvé, la séquence codante peut être reconstituée à partit du RBS
                builder = new StringBuilder();
                for (var i = rbsIndex; i < candidateSequence.Length; i++)
                {
                    builder.Append(candidateSequence[i]);
                }

                var dnaResult = builder.ToString();
                Gene gene = new Gene(dnaResult);
                gene.locus = initiationIndex;
                return gene;
            }
            else
            {
                return null;
            }
        }
        
        /// <summary>
        /// Transcription d'un gène en une séquence d'ARN Messager
        /// </summary>
        /// <param name="genome"></param>
        /// <returns></returns>
        public RiboNucleicAcid transcript(Gene genome)
        {
            StringBuilder rnaSequenceBuilder = new StringBuilder();
            foreach (var nucleotid in genome.GenomicSequence)
            {
                rnaSequenceBuilder.Append(getRNANucleotid(nucleotid.ToString()));
            }
            
            return new RiboNucleicAcid(rnaSequenceBuilder.ToString());
        } 
    }

}
