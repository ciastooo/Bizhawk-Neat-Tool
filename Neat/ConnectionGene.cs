using BizhawkNEAT.Utils;

namespace BizhawkNEAT.Neat
{
    public class ConnectionGene
    {
        public NodeGene PreviousNode { get; set; }
        public NodeGene NextNode { get; set; }
        public double Weight { get; set; }
        public bool IsDisabled { get; set; }

        public ConnectionGene(NodeGene previousNode, NodeGene nextNode)
        {
            PreviousNode = previousNode;
            NextNode = nextNode;
            Weight = RandomGenerator.NewWeight();
            IsDisabled = false;
        }

        public ConnectionGene(NodeGene previousNode, NodeGene nextNode, double weight)
        {
            PreviousNode = previousNode;
            NextNode = nextNode;
            Weight = weight;
            IsDisabled = false;
        }
    }
}
