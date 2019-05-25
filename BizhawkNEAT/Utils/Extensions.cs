using BizhawkNEAT.Neat;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BizhawkNEAT.Utils
{
    public static class Extensions
    {
        public static T GetRandomElement<T>(this IEnumerable<T> source)
        {
            var sourceList = source.ToList();
            var randomIndex = RandomGenerator.GetRandom().Next(sourceList.Count);
            return sourceList.ElementAtOrDefault(randomIndex);
        }

        public static ConnectionGene GetConnection(this IDictionary<int, ConnectionGene> sourceDictionary, NodeGene source, NodeGene target)
        {
            return sourceDictionary.Values.Where(x =>
                (x.PreviousNode.Id == source.Id && x.NextNode.Id == target.Id) ||
                (x.PreviousNode.Id == target.Id && x.NextNode.Id == source.Id))
                .FirstOrDefault();
        }

        public static double GetSpecieDistance(this Genome sourceGenome, Genome targetGenome)
        {
            var disjointNodes = 0;
            foreach (var sourceNodeGeneKey in sourceGenome.NodeGenes.Keys)
            {
                if (!targetGenome.NodeGenes.ContainsKey(sourceNodeGeneKey))
                {
                    disjointNodes += 1;
                }
            }

            foreach (var targetNodeGeneKey in targetGenome.NodeGenes.Keys)
            {
                if (sourceGenome.NodeGenes.ContainsKey(targetNodeGeneKey))
                {
                    disjointNodes += 1;
                }
            }
            var nodesDistance = Config.DisjointCoefficient * disjointNodes / Math.Max(sourceGenome.NodeGenes.Count, targetGenome.NodeGenes.Count);

            var connectionsDistance = 0d;
            if (sourceGenome.ConnectionGenes.Count > 0 || targetGenome.ConnectionGenes.Count > 0)
            {
                var weightDifference = 0d;
                var disjointConnections = 0;
                var matchingGenesCount = 0;
                foreach (var sourceConnectionGene in sourceGenome.ConnectionGenes)
                {
                    var sourceId = sourceConnectionGene.Key;
                    if (!targetGenome.ConnectionGenes.ContainsKey(sourceId))
                    {
                        disjointConnections += 1;
                    }
                    else
                    {
                        weightDifference += Math.Abs(sourceConnectionGene.Value.Weight - targetGenome.ConnectionGenes[sourceId].Weight);
                        matchingGenesCount += 1;
                    }
                }

                foreach (var targetId in targetGenome.ConnectionGenes.Keys)
                {
                    if (!sourceGenome.ConnectionGenes.ContainsKey(targetId))
                    {
                        disjointConnections++;
                    }
                }
                // Prevent dividing by 0
                if(matchingGenesCount < 1)
                {
                    matchingGenesCount = 1;
                }
                var averageWeightDifference = weightDifference / matchingGenesCount;

                connectionsDistance = Config.DisjointCoefficient * disjointConnections / Math.Max(sourceGenome.ConnectionGenes.Count, targetGenome.ConnectionGenes.Count);
                connectionsDistance += averageWeightDifference * Config.WeightCoefficient;
            }

            return nodesDistance + connectionsDistance;
        }

        public static bool IsSameSpecie(this Genome sourceGenome, Genome targetGenome)
        {
            return sourceGenome.GetSpecieDistance(targetGenome) < Config.SpecieThreshold;
        }
    }
}
