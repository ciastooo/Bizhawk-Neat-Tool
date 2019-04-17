using System.Collections.Generic;

namespace BizhawkNEAT.Neat
{
    public class Network
    {

        public Genome Crossover(Genome mother, Genome father)
        {
            var childGenome = new Genome();



            return childGenome;
        }

        private Dictionary<ConnectionGene, ConnectionGene> MatchGenomes(Genome mother, Genome father)
        {
            var resultMatch = new Dictionary<ConnectionGene, ConnectionGene>();

            foreach (var connectionGene in mother.ConnectionGenes)
            {

            }            

            return resultMatch;
        }
    }
}
