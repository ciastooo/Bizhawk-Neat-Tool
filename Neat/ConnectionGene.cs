using BizhawkNEAT.Utils;

namespace BizhawkNEAT.Neat
{
    public class ConnectionGene
    {
        public NodeGene PreviousNode { get; private set; }
        public NodeGene NextNode { get; private set; }
        public double Weight { get; private set; }
        public bool IsEnabled { get; private set; }

        public ConnectionGene(NodeGene previousNode, NodeGene nextNode)
        {
            PreviousNode = previousNode;
            NextNode = nextNode;
            Weight = RandomGenerator.NewWeight();
            IsEnabled = true;
        }

        public ConnectionGene(NodeGene previousNode, NodeGene nextNode, double weight)
        {
            PreviousNode = previousNode;
            NextNode = nextNode;
            Weight = weight;
            IsEnabled = true;
        }

        public ConnectionGene(ConnectionGene source)
        {
            PreviousNode = new NodeGene(source.PreviousNode);
            NextNode = new NodeGene(source.NextNode);
            Weight = source.Weight;
            IsEnabled = source.IsEnabled;
        }

        public void SetWeight(double newWeight)
        {
            Weight = newWeight;
        }

        public void Toggle(bool value)
        {
            IsEnabled = value;
        }
    }
}
