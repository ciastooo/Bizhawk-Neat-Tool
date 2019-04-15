using BizhawkNEAT.Helpers;
using System.Collections.Generic;

namespace BizhawkNEAT.Neat
{
    public class Genome
    {
        private IList<ConnectionGene> ConnectionGenes { get; set; }
        private IList<NodeGene> NodeGenes { get; set; }

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

            var newConnection = new ConnectionGene(ConnectionGenes.Count, firstNode, secondNode);
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

            var newPreviousConnection = new ConnectionGene(ConnectionGenes.Count, firstNode, newNode, 1);
            ConnectionGenes.Add(newPreviousConnection);

            var newNextConnection = new ConnectionGene(ConnectionGenes.Count, newNode, secondNode, connection.Weight);
            ConnectionGenes.Add(newNextConnection);
        }
    }
}
