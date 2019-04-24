namespace BizhawkNEAT.Neat
{
    public class NodeGene
    {
        public NodeGeneType Type { get; private set; }

        public NodeGene(NodeGeneType type = NodeGeneType.Hidden)
        {
            Type = type;
        }

        public NodeGene(NodeGene source)
        {
            Type = source.Type;
        }
    }
}
