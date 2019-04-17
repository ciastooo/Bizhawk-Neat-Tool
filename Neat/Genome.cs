using BizhawkNEAT.Utils;
using System.Collections.Generic;
using System.Linq;

namespace BizhawkNEAT.Neat
{
    public class Genome
    {
        public Dictionary<int, ConnectionGene> ConnectionGenes { get; set; }
        public Dictionary<int, NodeGene> NodeGenes { get; set; }

        public Genome()
        {
            ConnectionGenes = new Dictionary<int, ConnectionGene>();
            NodeGenes = new Dictionary<int, NodeGene>();
        }

        public void Mutate()
        {

        }

        public void AddConnection()
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
            ConnectionGenes.Add(IdGenerator.NextConnectionId(), newConnection);
        }

        public void AddNode()
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

            connection.IsDisabled = true;

            var newNode = new NodeGene();
            NodeGenes.Add(IdGenerator.NextNodeId(), newNode);

            var newPreviousConnection = new ConnectionGene(firstNode, newNode, 1);
            ConnectionGenes.Add(IdGenerator.NextConnectionId(), newPreviousConnection);

            var newNextConnection = new ConnectionGene(newNode, secondNode, connection.Weight);
            ConnectionGenes.Add(IdGenerator.NextConnectionId(), newNextConnection);
        }

        public void ToggleConnection(bool enable)
        {
            var toMutate = ConnectionGenes.Where(cg => cg.Value.IsDisabled == enable).GetRandomElement();
            toMutate.IsDisabled = enable;
        }

        public void AdjustWeight()
        {
            var toMutate = ConnectionGenes.Where(cg => !cg.Value.IsDisabled).GetRandomElement();
            toMutate.Weight = RandomGenerator.NewWeight(Config.Step);
        }

        public void RandomizeWeight()
        {
            var toMutate = ConnectionGenes.Where(cg => !cg.Value.IsDisabled).GetRandomElement();
            toMutate.Weight = RandomGenerator.NewWeight();
        }
    }
}
