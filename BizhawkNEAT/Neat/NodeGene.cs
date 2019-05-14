namespace BizhawkNEAT.Neat
{
    public class NodeGene
    {
        public NodeGeneType Type { get; private set; }

        public double Value { get; set; }

        public bool IsReady { get; set; }

        public NodeGene(NodeGeneType type = NodeGeneType.Hidden)
        {
            Type = type;
            Value = 0;
            IsReady = false;
        }

        public NodeGene(NodeGene source)
        {
            Type = source.Type;
            Value = source.Value;
            IsReady = source.IsReady;
        }
    }
}
