namespace BizhawkNEAT.Neat
{
    public class NodeGene
    {
        public NodeGeneType Type { get; set; }

        public NodeGene(NodeGeneType type = NodeGeneType.Hidden)
        {
            Type = type;
        }
    }
}
