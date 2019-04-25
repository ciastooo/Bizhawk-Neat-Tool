using BizhawkNEAT.Utils;
using System.Collections.Generic;
using System.Linq;

namespace BizhawkNEAT.Neat
{
    public class Network
    {
        public IList<Genome> Genomes { get; set; }

        public Genome Crossover(Genome mother, Genome father)
        {
            if (mother.Fitness < father.Fitness)
            {
                var tmp = father;
                father = mother;
                mother = tmp;
            }

            var childGenome = new Genome();

            foreach (var motherConnectionGene in mother.ConnectionGenes)
            {
                ConnectionGene newChildConnection;
                var coinToss = RandomGenerator.GetRandom().Next(0, 2);
                var connectionGeneId = motherConnectionGene.Key;

                if (father.ConnectionGenes.ContainsKey(connectionGeneId))
                {
                    if (coinToss == 0)
                    {
                        newChildConnection = new ConnectionGene(motherConnectionGene.Value);
                    }
                    else
                    {
                        newChildConnection = new ConnectionGene(father.ConnectionGenes[connectionGeneId]);
                    }
                }
                else if (mother.Fitness != father.Fitness || coinToss == 0)
                {
                    newChildConnection = new ConnectionGene(motherConnectionGene.Value);
                }
                else
                {
                    continue;
                }

                childGenome.AddConnectionGene(newChildConnection, connectionGeneId);
            }

            if (mother.Fitness == father.Fitness)
            {
                foreach (var fatherConnectionGene in father.ConnectionGenes.Where(x => !childGenome.ConnectionGenes.ContainsKey(x.Key)))
                {
                    var coinToss = RandomGenerator.GetRandom().Next(0, 2);
                    var connectionGeneId = fatherConnectionGene.Key;

                    if (coinToss == 1)
                    {
                        var newChildConnection = new ConnectionGene(fatherConnectionGene.Value);
                        childGenome.AddConnectionGene(newChildConnection, connectionGeneId);
                    }
                }
            }

            return childGenome;
        }

    }
}
