using BizhawkNEAT.Helpers;

namespace BizhawkNEAT.Neat
{
    public class ConnectionGene
    {
        public int Id { get; set; }
        public NodeGene PreviousNode { get; set; }
        public NodeGene NextNode { get; set; }
        public double Weight { get; set; }
        public bool IsDisabled { get; set; }

        public ConnectionGene(int newId, NodeGene previousNode, NodeGene nextNode)
        {
            Id = newId;
            PreviousNode = previousNode;
            NextNode = nextNode;
            Weight = RandomGenerator.NewWeight();
            IsDisabled = false;
        }

        public ConnectionGene(int newId, NodeGene previousNode, NodeGene nextNode, double weight)
        {
            Id = newId;
            PreviousNode = previousNode;
            NextNode = nextNode;
            Weight = weight;
            IsDisabled = false;
        }
    }
}
