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
            var randomIndex = RandomGenerator.GetRandom().Next(source.Count());
            return source.ElementAtOrDefault(randomIndex);
        }

        public static ConnectionGene GetConnection(this IDictionary<int, ConnectionGene> sourceDictionary, NodeGene source, NodeGene target)
        {
            return sourceDictionary.Values.Where(x =>
                (x.PreviousNode == source && x.NextNode == target) ||
                (x.PreviousNode == target && x.NextNode == source))
                .FirstOrDefault();
        }

        public static double GetSpecieDistance(this Genome sourceGenome, Genome targetGenome)
        {
            var disjointGenes = 0;
            var weightDifference = 0d;
            var matchingGenesCount = 0;

            var sourceGeneCount = 0;
            foreach (var sourceConnectionGene in sourceGenome.ConnectionGenes)
            {
                var sourceId = sourceConnectionGene.Key;
                sourceGeneCount++;
                if (!targetGenome.ConnectionGenes.ContainsKey(sourceId))
                {
                    disjointGenes++;
                }
                else
                {
                    weightDifference += Math.Abs(sourceConnectionGene.Value.Weight - targetGenome.ConnectionGenes[sourceId].Weight);
                    matchingGenesCount++;
                }
            }

            var targetGeneCount = 0;
            foreach (var targetId in targetGenome.ConnectionGenes.Keys)
            {
                targetGeneCount++;
                if (!sourceGenome.ConnectionGenes.ContainsKey(targetId))
                {
                    disjointGenes++;
                }
            }

            var geneCount = sourceGeneCount > targetGeneCount ? sourceGeneCount : targetGeneCount;
            if (geneCount < Config.SpecieSizeDelta)
            {
                geneCount = 1;
            }
            var averageWeightDifference = weightDifference / matchingGenesCount;

            return Config.DisjointDelta * disjointGenes / geneCount + Config.WeightDelta * averageWeightDifference;
        }

        public static bool IsSameSpecie(this Genome sourceGenome, Genome targetGenome)
        {
            return sourceGenome.GetSpecieDistance(targetGenome) < Config.SpecieThreshold;
        }
    }
}
