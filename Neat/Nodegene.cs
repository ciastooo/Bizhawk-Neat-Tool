namespace BizhawkNEAT.Neat
{
    public class NodeGene
    {
        public NodeGeneType Type { get; set; }
        public int Id { get; set; }

        public NodeGene(int newId, NodeGeneType type = NodeGeneType.Hidden)
        {
            Id = newId;
            Type = type;
        }
    }
}
