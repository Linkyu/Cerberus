using System;

namespace Module_3
{
    public class RiboNucleicAcid
    {
        public string Sequence { get; }

        public RiboNucleicAcid(string rnaSequence)
        {
            Sequence = (isRnaSequence(rnaSequence) ? rnaSequence : string.Empty);
        }

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