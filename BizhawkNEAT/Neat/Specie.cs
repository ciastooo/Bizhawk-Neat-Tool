using BizhawkNEAT.Utils;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace BizhawkNEAT.Neat
{
    public class Specie
    {
        public string Name { get; set; }
        public int TopFitness { get; set; }
        public int Staleness { get; set; }
        public double AverageFitness => Genomes.Average(g => g.Fitness);
        public double AverageGlobalRank => Genomes.Average(g => g.GlobalRank);

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

        public Specie(JToken json)
        {
            Genomes = new List<Genome>();
            Name = json.Value<string>("Name");
            Staleness = json.Value<int>("Staleness");
            foreach (var genomeJson in json["Genomes"].Children())
            {
                Genomes.Add(new Genome(genomeJson));
            }
        }

        public IList<Genome> GetMostFitGenomes()
        {
            var averageFitness = AverageFitness;
            var sortedGenomes = Genomes.OrderByDescending(g => g.Fitness).Take((Genomes.Count + 1) / 2).ToList();
            var mostFitGenomeFitnessInSpecie = sortedGenomes[0].Fitness;
            return sortedGenomes;
        }

        public Genome Breed()
        {
            Genome childGenome;

            if (Genomes.Count > 1 && RandomGenerator.GetRandomResult(Config.CrossoverChance))
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
                else
                {
                    newChildConnection = new ConnectionGene(motherConnectionGene.Value);
                }

                childGenome.AddConnectionGene(newChildConnection, connectionGeneId);
            }

            return childGenome;
        }

    }
}