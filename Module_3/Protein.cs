using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace Module_3
{
    public class Protein
    {
        private List<string> listAminoAcid;

        public Protein()
        {
            listAminoAcid = new List<string>();
        }

        public void AddAminoAcid(string aminoAcid)
        {
            listAminoAcid.Add(aminoAcid);
        }

        public override string ToString()
        {
            StringBuilder proteinBuilder = new StringBuilder();

            foreach (var aminoacid in listAminoAcid)
            {
                proteinBuilder.Append(aminoacid).Append("-");
            }

            return proteinBuilder.ToString();
        }
    }

    /*
     * RNAPolymerase is the protein witch makes RiboNucleic Acid from DNA
     */

    public class RnaPolymerase : Protein
    {
        public string dnaSequence { get; set; }

        public RnaPolymerase(string dnaSequence)
        {
            this.dnaSequence = dnaSequence;

        }
        
       /// <summary>
       ///  For each nucleotid received in parameter, return the RNA equivalent nucleotid.
       /// </summary>
       /// <param name="nucleotid"> The nucleotid we want the RNA equivalent.</param>
       /// <returns>The RNA equivalent nucleotid</returns>
        private static string GetRnaNucleotid(string nucleotid)
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
        /// Search a gene in the DNA sequence.
        /// </summary>
        /// <returns>Returns a gene with the nucleotidic sequence starting with the Ribosom Binding Site.</returns>
        public Gene SearchGenomicSequence()
        {
            var firstDnaStrand = dnaSequence;


            var firstStop = firstDnaStrand.IndexOf("TAA", new StringComparison());
            
            //Chercher le deuxieme codon stop
            Console.WriteLine("Premier stop : " + firstStop);
            var secondStop = firstDnaStrand.IndexOf("TAA", firstStop + 3, new StringComparison());

            var initiationIndex = -1;

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
                rbsIndex = firstDnaStrand.IndexOf(ribosomBindingSite, new StringComparison());
                if (rbsIndex > -1)
                {
                    rbsResult = ribosomBindingSite;
                }
            }
            
            Console.WriteLine(rbsIndex == -1
                ? "Aucun site de fixation du ribosome n'a été trouvé"
                : rbsIndex + " : un site de fixation a été trouvé.");

            //Chercher un start APRES le RBS  
            initiationIndex =
                firstDnaStrand.IndexOf("ATG", rbsIndex + rbsResult.Length - 1, new StringComparison());

            if (initiationIndex > -1 && (secondStop - firstStop -3) > 0 && (secondStop - (initiationIndex + 3)) % 3 == 0 )
            {
                //Un codon start a été trouvé, la séquence codante peut être reconstituée à partit du RBS
                Console.WriteLine("Test : Sequence trouvee.");
               var  builder = new StringBuilder();
                for (var i = rbsIndex; i < firstDnaStrand.Length; i++)
                {
                    builder.Append(firstDnaStrand[i]);
                }

                var dnaResult = builder.ToString();
                Gene gene = new Gene(dnaResult);
                gene.indexStart = initiationIndex;
                return gene;
            }
            else
            {
                return null;
            }
        }
        
        /// <summary>
        /// Transcript a gene into a RNA sequence.
        /// </summary>
        /// <param name="genome"></param>
        /// <returns></returns>
        public RiboNucleicAcid transcript(Gene genome)
        {
            StringBuilder rnaSequenceBuilder = new StringBuilder();
            foreach (var nucleotid in genome.GenomicSequence)
            {
                rnaSequenceBuilder.Append(GetRnaNucleotid(nucleotid.ToString()));
            }
            
            return new RiboNucleicAcid(rnaSequenceBuilder.ToString());
        } 
    }

}
