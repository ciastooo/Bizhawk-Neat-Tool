using BizhawkNEAT.Utils;
using System.Collections.Generic;
using System.Linq;

namespace BizhawkNEAT.Neat
{
    public class Specie
    {
        public string Name { get; set; }
        public double AverageFitness
        {
            get
            {
                return Genomes.Average(g => g.Fitness);
            }
        }
        public IList<Genome> Genomes { get; set; }

        public Genome Progenitor
        {
            get
            {
                return Genomes.First();
            }
        }

        public Specie(string name = null)
        {
            Genomes = new List<Genome>();
            Name = name ?? Config.GetNewSpecieName();
        }

        public IList<Genome> GetMostFitGenomes()
        {
            return Genomes.OrderByDescending(g => g.Fitness).Take(Genomes.Count / 2).ToList();
        }

        public Genome Breed()
        {
            Genome childGenome;

            if (RandomGenerator.GetRandomResult(Config.CrossoverChance) && Genomes.Count > 1)
            {
                childGenome = Crossover();
            }
            else
            {
                childGenome = new Genome(Genomes.GetRandomElement());
            }

            childGenome.TryMutate();

            return childGenome;
        }

        private Genome Crossover()
        {
            var mother = Genomes.GetRandomElement();
            var father = Genomes.GetRandomElement();

            while (father == mother)
            {
                father = Genomes.GetRandomElement();
            }

            if (mother.Fitness < father.Fitness)
            {
                var tmp = father;
                father = mother;
                mother = tmp;
            }

            var childGenome = new Genome();

            foreach (var node in mother.NodeGenes.Values)
            {
                childGenome.AddNodeGene(new NodeGene(node));
            }

            foreach (var node in father.NodeGenes.Values)
            {
                if (!childGenome.NodeGenes.ContainsKey(node.Id))
                {
                    childGenome.AddNodeGene(new NodeGene(node));
                }
            }

            foreach (var motherConnectionGene in mother.ConnectionGenes)
            {
                ConnectionGene newChildConnection;
                var coinToss = RandomGenerator.GetCoinToss();
                var connectionGeneId = motherConnectionGene.Key;

                if (father.ConnectionGenes.ContainsKey(connectionGeneId))
                {
                    if (coinToss)
                    {
                        newChildConnection = new ConnectionGene(motherConnectionGene.Value);
                    }
                    else
                    {
                        newChildConnection = new ConnectionGene(father.ConnectionGenes[connectionGeneId]);
                    }
                }
                else if (mother.Fitness != father.Fitness || coinToss)
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
                    var coinToss = RandomGenerator.GetCoinToss();

                    if (coinToss)
                    {
                        var connectionGeneId = fatherConnectionGene.Key;
                        var newChildConnection = new ConnectionGene(fatherConnectionGene.Value);
                        childGenome.AddConnectionGene(newChildConnection, connectionGeneId);
                    }
                }
            }

            return childGenome;
        }

    }
}