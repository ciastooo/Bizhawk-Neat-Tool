using BizhawkNEAT.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BizhawkNEAT.Neat
{
    public class Genome
    {
        public Dictionary<int, ConnectionGene> ConnectionGenes { get; private set; }
        public Dictionary<int, NodeGene> NodeGenes { get; private set; }

        public List<NodeGene> InputNodes => NodeGenes.Where(ng => ng.Value.Type == NodeGeneType.Input).Select(ng => ng.Value).ToList();
        public List<NodeGene> OutputNodes => NodeGenes.Where(ng => ng.Value.Type == NodeGeneType.Output).Select(ng => ng.Value).ToList();

        public int Fitness { get; set; }

        public Genome()
        {
            ConnectionGenes = new Dictionary<int, ConnectionGene>();
            NodeGenes = new Dictionary<int, NodeGene>();
        }

        public bool[] Propagate(double[] inputs)
        {
            var inputNodes = InputNodes;
            if(inputs.Length != inputNodes.Count)
            {
                throw new Exception("Number of inputs does not match number of genome inputs");
            }

            foreach (var node in NodeGenes.Values)
            {
                node.Value = 0;
                node.IsReady = false;
            }

            for (int i = 0; i < inputs.Length; i++)
            {
                inputNodes[i].Value = inputs[i];
                inputNodes[i].IsReady = true;
            }

            var connectionsToPropagate = ConnectionGenes.Values.Where(cg => cg.IsEnabled).ToList();

            while(connectionsToPropagate.Count != 0)
            {
                var propagatedConnections = new List<ConnectionGene>();

                foreach(var connectionGene in connectionsToPropagate)
                {
                    if(!connectionGene.PreviousNode.IsReady)
                    {
                        continue;
                    }

                    connectionGene.NextNode.Value += connectionGene.PreviousNode.Value * connectionGene.Weight;
                    propagatedConnections.Add(connectionGene);

                    if(!connectionsToPropagate.Any(cg => cg.NextNode == connectionGene.NextNode && cg != connectionGene))
                    {
                        connectionGene.NextNode.Value = ActivationFunctions.Sigmoid.Count(connectionGene.NextNode.Value);
                        connectionGene.NextNode.IsReady = true;
                    }
                }

                foreach (var propagatedConnectionGene in propagatedConnections)
                {
                    connectionsToPropagate.Remove(propagatedConnectionGene);
                }
            }

            return OutputNodes.Select(x => x.Value > 0).ToArray();
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
            while (firstNode == secondNode ||
                   ConnectionGenes.GetConnection(firstNode, secondNode) != null ||
                   firstNode.Type == NodeGeneType.Input && secondNode.Type == NodeGeneType.Input ||
                   firstNode.Type == NodeGeneType.Output && secondNode.Type == NodeGeneType.Output)
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

        public void MutateAdjustWeight(bool randomizeweight = false)
        {
            var toMutate = ConnectionGenes.Where(cg => cg.Value.IsEnabled).GetRandomElement();
            if (randomizeweight)
            {
                toMutate.SetWeight(RandomGenerator.NewWeight());
            }
            else
            {
                toMutate.SetWeight(RandomGenerator.NewWeight(Config.WeightStep));
            }
        }

        public void MutateDeleteConnection()
        {
            var connectionToDelete = NodeGenes.GetRandomElement();

            NodeGenes.Remove(NodeGenes.First(x => x.Value == connectionToDelete).Key);
        }
    }
}
