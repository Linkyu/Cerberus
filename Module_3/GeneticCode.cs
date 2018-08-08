﻿using System;
using System.Collections.Generic;

namespace Module_3
{
    public class GeneticCode
    {
        private static Dictionary<string, string> geneticCode = new Dictionary<string, string>
            {
                ["UUU"] = "Phenilalaline",
                ["UUC"] = "Phenilalanine",
                ["UUA"] = "Leucine",
                ["UUG"] = "Leucine",
                ["CUU"] = "Leucine",
                ["CUC"] = "Leucine",
                ["CUA"] = "Leucine",
                ["CUG"] = "Leucine",
                ["AUU"] = "Isoleucine",
                ["AUC"] = "Isoleucine",
                ["AUA"] = "Isoleucine",
                ["AUG"] = "Methionine",
                ["GUU"] = "Valine",
                ["GUC"] = "Valine",
                ["GUA"] = "Valine",
                ["GUG"] = "Valine",
                ["UCU"] = "Serine",
                ["UCC"] = "Serine",
                ["UCA"] = "Serine",
                ["UCG"] = "Serine",
                ["CCU"] = "Proline",
                ["CCC"] = "Proline",
                ["CCA"] = "Proline",
                ["CCG"] = "Proline",
                ["ACU"] = "Threonine",
                ["ACC"] = "Threonine",
                ["ACA"] = "Threonine",
                ["ACG"] = "Theonine",
                ["GCU"] = "Alanine",
                ["GCC"] = "Alanine",
                ["GCA"] = "Alanine",
                ["GCG"] = "Alanine",
                ["UAU"] = "Thyrosine",
                ["UAC"] = "Thyrosine",
                ["UAA"] = "STOP",
                ["UAG"] = "STOP",
                ["CAU"] = "Histidine",
                ["CAC"] = "Histidine",
                ["CAA"] = "Glutamine",
                ["CAG"] = "Glutamine",
                ["AAU"] = "Asparagine",
                ["AAC"] = "Asparagine",
                ["AAA"] = "Lysine",
                ["AAG"] = "Lysine",
                ["GAU"] = "Acide aspartique",
                ["GAC"] = "Acide aspartique",
                ["GAA"] = "Acide glutamique",
                ["GAG"] = "Acide glutamique",
                ["UGU"] = "Cysteine",
                ["UGC"] = "Cysteine",
                ["UGA"] = "STOP",
                ["UGG"] = "Tryptophane",
                ["CGU"] = "Arginine",
                ["CGC"] = "Arginine",
                ["CGA"] = "Arginine",
                ["CGG"] = "Arginine",
                ["AGU"] = "Serine",
                ["AGC"] = "Serine",
                ["AGA"] = "Arginine",
                ["AGG"] = "Arginine",
                ["GCU"] = "Glycine",
                ["GGC"] = "Glycine",
                ["GGA"] = "Glycine",
                ["GGG"] = "Glycine"
            };

        public static string getAminoAcid(string codon) {
            if (codon.Length != 3) {
                throw new Exception();
            }
            return geneticCode[codon];

        }
    }
}