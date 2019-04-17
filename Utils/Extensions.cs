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
    }
}
