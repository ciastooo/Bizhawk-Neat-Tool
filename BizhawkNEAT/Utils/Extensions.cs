using BizhawkNEAT.Neat;
using System.Collections.Generic;
using System.Linq;

namespace BizhawkNEAT.Utils
{
    public static class Extensions
    {
        public static T GetRandomElement<T>(this IEnumerable<KeyValuePair<int, T>> source)
        {
            var randomIndex = RandomGenerator.GetRandom().Next(source.Count());
            return source.First(x => x.Key == randomIndex).Value;
        }

        public static ConnectionGene GetConnection(this IDictionary<int, ConnectionGene> sourceDictionary, NodeGene source, NodeGene target)
        {
            return sourceDictionary.Where(x =>
                (x.Value.PreviousNode == source && x.Value.NextNode == target) ||
                (x.Value.PreviousNode == target && x.Value.NextNode == source))
                .Select(x => x.Value).FirstOrDefault();
        }

        public static double GetSpecieDistance(this IDictionary<int, ConnectionGene> sourceDictionary, IDictionary<int, ConnectionGene> targetDictionary)
        {
            var geneCount = 0;
            var disjointGenes = 0;
            var excessGenes = 0;
            var weightDifference = 0d;


            var targetMaxId = 0;
            var targetGeneCount = 0;
            foreach (var targetGeneKey in targetDictionary.Keys)
            {
                targetGeneCount++;
                if (targetGeneKey > targetMaxId)
                {
                    targetMaxId = targetGeneKey;
                };
            }

            var sourceMaxId = 0;
            foreach (var sourceGeneKey in sourceDictionary.Keys)
            {
                geneCount++;

                if (sourceGeneKey > sourceMaxId)
                {
                    sourceMaxId = sourceGeneKey;
                };

                if (!targetDictionary.ContainsKey(sourceGeneKey))
                {
                    if(sourceGeneKey < targetMaxId)
                    {
                        disjointGenes++;
                    } else
                    {
                        excessGenes++;
                    }
                }
            }

            foreach (var targetGeneKey in targetDictionary.Keys)
            {
                if (!sourceDictionary.ContainsKey(targetGeneKey))
                {
                    if (targetGeneKey < sourceMaxId)
                    {
                        disjointGenes++;
                    }
                    else
                    {
                        excessGenes++;
                    }
                }
            }

            if (targetGeneCount > geneCount)
            {
                geneCount = targetGeneCount;
            }

            return ((Config.C1 * excessGenes) + (Config.C2 * disjointGenes)) / geneCount + Config.C3 * weightDifference;
        }
    }
}
