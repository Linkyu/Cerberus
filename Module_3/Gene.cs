namespace Module_3
{
    public class Gene
    {
        
        public string GenomicSequence { get; set; }
        
        //the index of the gene in the sequence
        public int indexStart;
        
        public Gene(string sequence)
        {
            GenomicSequence = sequence;
        }
        
        
    }
}