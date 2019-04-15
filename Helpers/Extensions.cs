using BizhawkNEAT.Neat;
using System.Collections.Generic;
using System.Linq;

namespace BizhawkNEAT.Helpers
{
    public static class Extensions
    {
        public static T GetRandomElement<T>(this IEnumerable<T> source)
        {
            var random = RandomGenerator.GetRandom();
            var list = source.ToList();
            return list[random.Next(list.Count)];
        }

        public static ConnectionGene GetConnection(this IList<ConnectionGene> list, NodeGene source, NodeGene target)
        {
            return list.FirstOrDefault(x => (x.PreviousNode == source && x.NextNode == target) || (x.PreviousNode == target && x.NextNode == source));
        }
    }
}
