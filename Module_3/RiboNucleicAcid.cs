using System;

namespace Module_3
{
    public class RiboNucleicAcid
    {
        //The RiboNucleicAcid Sequence with an alphabet of four letters.
        public string Sequence { get; }

        /// <summary>
        /// Build a RNA sequence composed by an alphabet of four letters(A, U, G, C)
        /// </summary>
        /// <param name="rnaSequence"> The nucleotidic sequence that must be a RNA sequence.</param>
        public RiboNucleicAcid(string rnaSequence)
        {
            Sequence = (isRnaSequence(rnaSequence) ? rnaSequence : string.Empty);
        }

        /// <summary>
        /// Check the string in parameter is a RNA sequence with only 4 letter possible (A, U, C, G)
        /// </summary>
        /// <param name="sequence">The string to be checked</param>
        /// <returns>true if the given sequence is RNA sequence, false otherwise.</returns>
        private static bool isRnaSequence(string sequence)
        {
            var nbrAdenine = 0;
            var nbrGuanine = 0;
            var nbrCitosine = 0;
            var nbrUracile = 0;
            var somme = 0; 
            for (var index = 0; index < sequence.Length; index++)
            {
                switch (sequence[index])
                {
                    case 'A':
                        nbrAdenine++;
                        break;
                    case 'C':
                        nbrCitosine++;
                        break;
                    case 'U' :
                        nbrUracile++;
                        break;
                    case 'G':
                        nbrGuanine++;
                        break;
                    default:
                        somme++;
                        break;
                }
            }

            somme += somme + (nbrAdenine + nbrCitosine + nbrGuanine + nbrUracile);
            return  somme == sequence.Length;
        }
      
    }
}