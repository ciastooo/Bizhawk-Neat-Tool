using BizhawkNEAT.Neat;
using System.Linq;

namespace BizhawkNEAT.Utils
{
    public static class MutationExtensions
    {
        public static void MutateAddConnection(this Genome genome)
        {
            var firstNode = genome.NodeGenes.GetRandomElement();
            var secondNode = genome.NodeGenes.GetRandomElement();
            while (firstNode == secondNode ||
                   genome.ConnectionGenes.GetConnection(firstNode, secondNode) != null ||
                   firstNode.Type == NodeGeneType.Input && secondNode.Type == NodeGeneType.Input ||
                   firstNode.Type == NodeGeneType.Output && secondNode.Type == NodeGeneType.Output)
            {
                secondNode = genome.NodeGenes.GetRandomElement();
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
            genome.AddConnectionGene(newConnection);
        }

        public static void MutateAddNode(this Genome genome)
        {
            var firstNode = genome.NodeGenes.GetRandomElement();
            var secondNode = genome.NodeGenes.GetRandomElement();
            var connection = genome.ConnectionGenes.GetConnection(firstNode, secondNode);

            while (firstNode == secondNode || connection == null)
            {
                secondNode = genome.NodeGenes.GetRandomElement();
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
            genome.AddNodeGene(newNode);

            var newPreviousConnection = new ConnectionGene(firstNode, newNode, 1);
            genome.AddConnectionGene(newPreviousConnection);

            var newNextConnection = new ConnectionGene(newNode, secondNode, connection.Weight);
            genome.AddConnectionGene(newNextConnection);
        }

        public static void MutateToggleConnection(this Genome genome, bool enable)
        {
            var toMutate = genome.ConnectionGenes.Where(cg => cg.Value.IsEnabled == !enable).GetRandomElement();
            toMutate.Toggle(enable);
        }

        public static void MutateAdjustWeight(this Genome genome, bool randomizeweight = false)
        {
            var toMutate = genome.ConnectionGenes.Where(cg => cg.Value.IsEnabled).GetRandomElement();
            if (randomizeweight)
            {
                toMutate.SetWeight(RandomGenerator.NewWeight());
            }
            else
            {
                toMutate.SetWeight(RandomGenerator.NewWeight(Config.WeightStep));
            }
        }

        public static void MutateDeleteConnection(this Genome genome)
        {
            var connectionToDelete = genome.NodeGenes.GetRandomElement();

            genome.NodeGenes.Remove(genome.NodeGenes.First(x => x.Value == connectionToDelete).Key);
        }
    }
}
