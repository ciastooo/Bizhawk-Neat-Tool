using BizhawkNEAT.Utils;
using Newtonsoft.Json.Linq;
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
        public List<NodeGene> HiddenNodes => NodeGenes.Where(ng => ng.Value.Type == NodeGeneType.Hidden).Select(ng => ng.Value).ToList();
        public List<NodeGene> OutputNodes => NodeGenes.Where(ng => ng.Value.Type == NodeGeneType.Output).Select(ng => ng.Value).ToList();

        public int Fitness { get; set; }
        public int GlobalRank { get; set; }

        public Genome()
        {
            ConnectionGenes = new Dictionary<int, ConnectionGene>();
            NodeGenes = new Dictionary<int, NodeGene>();
        }

        public Genome(Genome toCopy): this()
        {
            foreach(var nodeGene in toCopy.NodeGenes.Values)
            {
                AddNodeGene(new NodeGene(nodeGene));
            }

            foreach (var connectionGene in toCopy.ConnectionGenes)
            {
                AddConnectionGene(new ConnectionGene(connectionGene.Value), connectionGene.Key);
            }
        }

        public Genome(JToken json)
        {
            ConnectionGenes = new Dictionary<int, ConnectionGene>();
            NodeGenes = new Dictionary<int, NodeGene>();
            foreach (var nodeJson in json["NodeGenes"].Children())
            {
                var node = new NodeGene(nodeJson);
                AddNodeGene(node);
            }
            foreach (var connectionJson in json["ConnectionGenes"].Children())
            {
                var id = connectionJson.Value<int>("Key");
                var value = connectionJson.Value<JToken>("Value");

                var connection = new ConnectionGene(value);

                AddConnectionGene(connection, id);
            }
        }

        public bool[] Propagate(int[] inputs)
        {
            var inputNodes = InputNodes;
            if (inputs.Length != inputNodes.Count)
            {
                throw new Exception("Number of inputs does not match number of genome inputs");
            }

            var connectionsToPropagate = ConnectionGenes.Values.Where(cg => cg.IsEnabled).ToList();

            if(connectionsToPropagate.Count == 0)
            {
                return OutputNodes.Select(x => x.Value > 0).ToArray();
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


            while (connectionsToPropagate.Count != 0)
            {
                var propagatedConnections = new List<ConnectionGene>();

                foreach (var connectionGene in connectionsToPropagate)
                {
                    var previouesNode = NodeGenes[connectionGene.PreviousNodeId];
                    if (!previouesNode.IsReady)
                    {
                        continue;
                    }
                    var nextNode = NodeGenes[connectionGene.NextNodeId];

                    nextNode.Value += previouesNode.Value * connectionGene.Weight;
                    propagatedConnections.Add(connectionGene);

                    if (!connectionsToPropagate.Any(cg => cg.NextNodeId == connectionGene.NextNodeId && cg != connectionGene))
                    {
                        nextNode.Value = ActivationFunctions.Sigmoid.Count(nextNode.Value);
                        nextNode.IsReady = true;
                    }
                }

                // No changes in propagation; Input values cannot be passed further
                if(propagatedConnections.Count == 0)
                {
                    break;
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
            if (!NodeGenes.ContainsKey(index))
            {
                NodeGenes.Add(index, toAdd);
            }
        }

        public void AddNodeGene(NodeGene toAdd)
        {
            if (!NodeGenes.ContainsKey(toAdd.Id))
            {
                NodeGenes.Add(toAdd.Id, toAdd);
            }
        }

        public void TryMutate()
        {
            if (RandomGenerator.GetRandomResult(Config.MutationAddConnectionProbability))
            {
                this.MutateAddConnection();
            }

            if (RandomGenerator.GetRandomResult(Config.MutationAddConnectionToBiasProbability))
            {
                this.MutateAddConnection(true);
            }

            if (RandomGenerator.GetRandomResult(Config.MutationAddNodeProbability))
            {
                this.MutateAddNode();
            }

            if (RandomGenerator.GetRandomResult(Config.MutationAdjustWeightProbability))
            {
                this.MutateAdjustWeight();
            }

            if (RandomGenerator.GetRandomResult(Config.MutationDisableConnectionProbability))
            {
                this.MutateToggleConnection(false);
            }

            if (RandomGenerator.GetRandomResult(Config.MutationEnableConnectionProbability))
            {
                this.MutateToggleConnection(true);
            }
        }
    }
}