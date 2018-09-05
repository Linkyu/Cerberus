using System;
using System.Collections.Generic;
using System.Text;

namespace Module_3
{
    public class Ribosom{


        public Protein translate(RiboNucleicAcid acid, int startIndex)
        {
            if (!CheckRbs(acid))
            {
                throw new Exception("No RBS detected.");
            }
            var listCodon = new List<string>();
            var protein = new Protein();
            StringBuilder codonBuilder = new StringBuilder();
            for (var i = startIndex + 3; i < acid.Sequence.Length - 3; i++)
            {
                codonBuilder.Append(acid.Sequence[i]);    
                if (i % 3 == 0)
                {
                    Console.WriteLine(codonBuilder.ToString());
                    listCodon.Add(codonBuilder.ToString());
                    codonBuilder = new StringBuilder();
                }
            }

            foreach (var codon in listCodon)
            {
                var aminoAcid = GeneticCode.getAminoAcid(codon);
                protein.AddAminoAcid(aminoAcid);
            }

            return protein;
        }
         
        /// <summary>
        /// 
        /// </summary>
        /// <param name="acid"></param>
        /// <returns></returns>
        private static bool CheckRbs(RiboNucleicAcid acid) {
            
            var listRbs = new List<string>();
            listRbs.Add("AGGAGG");
            listRbs.Add("AGGA");
            listRbs.Add("GAGG");
            listRbs.Add("GGAG");

            //Search a Ribosom Binding Site
            var rbsIndex = 0;
            var rbsResult = "";
            foreach (var ribosomBindingSite in listRbs)
            {
                rbsIndex = acid.Sequence.IndexOf(ribosomBindingSite, new StringComparison());
                if (rbsIndex > -1)
                {
                    rbsResult = ribosomBindingSite;
                }
            }
            //Check the Ribosom Biding  length is greather than zero and located at the begining of the sequence.
            return rbsResult.Length > 0 && rbsIndex == 0;
        }
    }
}