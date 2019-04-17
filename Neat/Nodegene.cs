namespace BizhawkNEAT.Neat
{
    public class NodeGene
    {
        public int Id { get; set; }
        public NodeGeneType Type { get; set; }

        public NodeGene(int newId, NodeGeneType type = NodeGeneType.Hidden)
        {
            Id = newId;
            Type = type;
        }
    }
}
