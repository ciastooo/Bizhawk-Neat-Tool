using BizhawkNEAT.Utils;
using System.Collections.Generic;
using System.Linq;

namespace BizhawkNEAT.Neat
{
    public class Genome
    {
        public Dictionary<int, ConnectionGene> ConnectionGenes { get; private set; }
        public Dictionary<int, NodeGene> NodeGenes { get; private set; }

        public int Fitness { get; set; }

        public Genome()
        {
            ConnectionGenes = new Dictionary<int, ConnectionGene>();
            NodeGenes = new Dictionary<int, NodeGene>();
        }

        public void AddConnectionGene(ConnectionGene toAdd, int index)
        {
            ConnectionGenes.Add(index, toAdd);
        }

        public void AddConnectionGene(ConnectionGene toAdd)
        {
            ConnectionGenes.Add(IdGenerator.NextConnectionId(), toAdd);
        }

        public void AddNodeGene(NodeGene toAdd, int index)
        {
            NodeGenes.Add(index, toAdd);
        }

        public void AddNodeGene(NodeGene toAdd)
        {
            NodeGenes.Add(IdGenerator.NextNodeId(), toAdd);
        }

        public void MutateAddConnection()
        {
            var firstNode = NodeGenes.GetRandomElement();
            var secondNode = NodeGenes.GetRandomElement();
            while (firstNode == secondNode || ConnectionGenes.GetConnection(firstNode, secondNode) != null)
            {
                secondNode = NodeGenes.GetRandomElement();
            }

            if (firstNode.Type == NodeGeneType.Output && secondNode.Type == NodeGeneType.Hidden ||
               firstNode.Type == NodeGeneType.Hidden && secondNode.Type == NodeGeneType.Input ||
               firstNode.Type == NodeGeneType.Output && secondNode.Type == NodeGeneType.Input)
            {
                var tmp = secondNode;
                secondNode = firstNode;
                firstNode = tmp;
            }

            var newConnection = new ConnectionGene(firstNode, secondNode);
            AddConnectionGene(newConnection);
        }

        public void MutateAddNode()
        {
            var firstNode = NodeGenes.GetRandomElement();
            var secondNode = NodeGenes.GetRandomElement();
            var connection = ConnectionGenes.GetConnection(firstNode, secondNode);

            while (firstNode == secondNode || connection == null)
            {
                secondNode = NodeGenes.GetRandomElement();
            }

            if (firstNode.Type == NodeGeneType.Output && secondNode.Type == NodeGeneType.Hidden ||
               firstNode.Type == NodeGeneType.Hidden && secondNode.Type == NodeGeneType.Input ||
               firstNode.Type == NodeGeneType.Output && secondNode.Type == NodeGeneType.Input)
            {
                var tmp = secondNode;
                secondNode = firstNode;
                firstNode = tmp;
            }

            connection.Toggle(true);

            var newNode = new NodeGene();
            AddNodeGene(newNode);

            var newPreviousConnection = new ConnectionGene(firstNode, newNode, 1);
            AddConnectionGene(newPreviousConnection);

            var newNextConnection = new ConnectionGene(newNode, secondNode, connection.Weight);
            AddConnectionGene(newNextConnection);
        }

        public void MutateToggleConnection(bool enable)
        {
            var toMutate = ConnectionGenes.Where(cg => cg.Value.IsEnabled == !enable).GetRandomElement();
            toMutate.Toggle(enable);
        }

        public void MutateAdjustWeight()
        {
            var toMutate = ConnectionGenes.Where(cg => cg.Value.IsEnabled).GetRandomElement();
            toMutate.SetWeight(RandomGenerator.NewWeight(Config.Step));
        }

        public void MutateRandomizeWeight()
        {
            var toMutate = ConnectionGenes.Where(cg => cg.Value.IsEnabled).GetRandomElement();
            toMutate.SetWeight(RandomGenerator.NewWeight());
        }
    }
}
