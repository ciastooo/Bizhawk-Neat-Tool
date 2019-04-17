using BizhawkNEAT.Utils;
using System.Collections.Generic;
using System.Linq;

namespace BizhawkNEAT.Neat
{
    public class Genome
    {
        public IList<ConnectionGene> ConnectionGenes { get; set; }
        public IList<NodeGene> NodeGenes { get; set; }

        public Genome()
        {
            ConnectionGenes = new List<ConnectionGene>();
            NodeGenes = new List<NodeGene>();
        }

        public void Mutate()
        {

        }

        public void AddConnection()
        {
            var firstNode = NodeGenes.GetRandomElement();
            var secondNode = NodeGenes.GetRandomElement();
            while(firstNode == secondNode || ConnectionGenes.GetConnection(firstNode, secondNode) != null)
            {
                secondNode = NodeGenes.GetRandomElement();
            }

            if(firstNode.Type == NodeGeneType.Output && secondNode.Type == NodeGeneType.Hidden ||
               firstNode.Type == NodeGeneType.Hidden && secondNode.Type == NodeGeneType.Input ||
               firstNode.Type == NodeGeneType.Output && secondNode.Type == NodeGeneType.Input)
            {
                var tmp = secondNode;
                secondNode = firstNode;
                firstNode = tmp;
            }

            var newConnection = new ConnectionGene(IdGenerator.Next(), firstNode, secondNode);
            ConnectionGenes.Add(newConnection);
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

            var newNode = new NodeGene(NodeGenes.Count);
            NodeGenes.Add(newNode);

            var newPreviousConnection = new ConnectionGene(IdGenerator.Next(), firstNode, newNode, 1);
            ConnectionGenes.Add(newPreviousConnection);

            var newNextConnection = new ConnectionGene(IdGenerator.Next(), newNode, secondNode, connection.Weight);
            ConnectionGenes.Add(newNextConnection);
        }

        public void ToggleConnection(bool enable)
        {
            var toMutate = ConnectionGenes.Where(cg => cg.IsDisabled == enable).GetRandomElement();
            toMutate.IsDisabled = enable;
        }

        public void AdjustWeight()
        {
            var toMutate = ConnectionGenes.Where(cg => !cg.IsDisabled).GetRandomElement();
            toMutate.Weight = RandomGenerator.NewWeight(Config.Step);
        }

        public void RandomizeWeight()
        {
            var toMutate = ConnectionGenes.Where(cg => !cg.IsDisabled).GetRandomElement();
            toMutate.Weight = RandomGenerator.NewWeight();
        }
    }
}
