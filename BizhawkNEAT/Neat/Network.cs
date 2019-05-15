using BizhawkNEAT.Utils;
using System.Collections.Generic;

namespace BizhawkNEAT.Neat
{
    public class Network
    {
        public IList<Specie> Species { get; set; }

        public int Generation { get; set; }

        public Network()
        {
            Generation = 0;
            Species = new List<Specie>();
        }

        public void Init(int inputNodesCount, int outputNodesCount)
        {
            var initSpecie = new Specie();

            for (int i = 0; i < Config.Population; i++)
            {
                var newGenome = new Genome();
                for (int j = 0; j < inputNodesCount; j++)
                {
                    var inputNode = new NodeGene(NodeGeneType.Input);
                    newGenome.AddNodeGene(inputNode, inputNode.Id);
                }
                for (int j = 0; j < outputNodesCount; j++)
                {
                    var outputNode = new NodeGene(NodeGeneType.Output);
                    newGenome.AddNodeGene(outputNode, outputNode.Id);
                }

                initSpecie.Genomes.Add(newGenome);
            }
        }

        private void NextGeneration()
        {
            var newGeneration = new List<Specie>();
            var newGenerationCount = 0;
            var totalAverageFitness = 0d;

            foreach (var specie in Species)
            {
                var childSpecie = new Specie(specie.Name);
                childSpecie.Genomes = specie.GetMostFitGenomes();
                if (childSpecie.Genomes.Count > 0)
                {
                    newGeneration.Add(childSpecie);
                    newGenerationCount += childSpecie.Genomes.Count;
                    totalAverageFitness += specie.AverageFitness;
                }
            }

            var nextGenerationChildrenGenomes = new List<Genome>();
            foreach (var specie in Species)
            {
                var childrenGenomesTobreed = (int)(specie.AverageFitness / totalAverageFitness * (Config.Population - newGenerationCount) - 1);
                for (int i = 0; i < childrenGenomesTobreed; i++)
                {
                    nextGenerationChildrenGenomes.Add(specie.Breed());
                }
            }

            while (newGenerationCount + nextGenerationChildrenGenomes.Count < Config.Population)
            {
                var randomSpecie = Species.GetRandomElement();
                nextGenerationChildrenGenomes.Add(randomSpecie.Breed());
            }

            foreach (var childrenGenome in nextGenerationChildrenGenomes)
            {
                AssignSpecie(newGeneration, childrenGenome);
            }

            Generation++;
            Species = newGeneration;
        }

        private void AssignSpecie(IList<Specie> species, Genome genomeToAdd)
        {
            foreach (var specie in species)
            {
                if (genomeToAdd.IsSameSpecie(specie.Progenitor))
                {
                    specie.Genomes.Add(genomeToAdd);
                    return;
                }
            }

            var newSpecie = new Specie();
            newSpecie.Genomes.Add(genomeToAdd);
        }
    }
}
