using BizhawkNEAT.Neat;
using System.Linq;

namespace BizhawkNEAT.Utils
{
    public static class MutationExtensions
    {
        public static void MutateAddConnection(this Genome genome, bool forceBias = false)
        {
            NodeGene firstNode;
            if(forceBias)
            {
                firstNode = genome.NodeGenes.Values.Where(g => g.Type == NodeGeneType.Input).Last();
            } else
            {
                firstNode = genome.NodeGenes.Values.GetRandomElement();
            }

            var secondNode = genome.NodeGenes.Values.GetRandomElement();
            while (firstNode.Id == secondNode.Id ||
                   genome.ConnectionGenes.GetConnection(firstNode, secondNode) != null ||
                   firstNode.Type == NodeGeneType.Input && secondNode.Type == NodeGeneType.Input ||
                   firstNode.Type == NodeGeneType.Output && secondNode.Type == NodeGeneType.Output)
            {
                secondNode = genome.NodeGenes.Values.GetRandomElement();
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
            var connection = genome.ConnectionGenes.Values.Where(cg => cg.IsEnabled).GetRandomElement();
            if (connection == null)
            {
                return;
            }

            var firstNode = connection.PreviousNode;
            var secondNode = connection.NextNode;

            var newNode = new NodeGene();
            genome.AddNodeGene(newNode);

            var newPreviousConnection = new ConnectionGene(firstNode, newNode, 1);
            genome.AddConnectionGene(newPreviousConnection);

            var newNextConnection = new ConnectionGene(newNode, secondNode, connection.Weight);
            genome.AddConnectionGene(newNextConnection);

            connection.Toggle(false);
        }

        public static void MutateToggleConnection(this Genome genome, bool enable)
        {
            var toMutate = genome.ConnectionGenes.Values.Where(cg => cg.IsEnabled == !enable).GetRandomElement();
            if (toMutate == null)
            {
                return;
            }

            toMutate.Toggle(enable);
        }

        public static void MutateAdjustWeight(this Genome genome)
        {
            foreach (var connection in genome.ConnectionGenes.Values)
            {
                if (RandomGenerator.GetRandomResult(Config.MutationPerturbateWeightProbability))
                {
                    connection.SetWeight(RandomGenerator.NewWeight());
                } else
                {
                    connection.SetWeight(connection.Weight + RandomGenerator.NewWeight(Config.WeightStep));
                }
            }
        }
    }
}
