using BizhawkNEAT.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BizhawkNEAT.Neat
{
    public class Network
    {
        private readonly GameInformationHandler _gameInformationHandler;

        public IList<Specie> Species { get; set; }

        public int Generation { get; set; }

        private int CurrentFrame { get; set; }

        private int Timeout { get; set; }

        private int RightmostPosition { get; set; }

        private Specie CurrentSpecie { get; set; }
        private Genome CurrentPlayer { get; set; }

        public Network(GameInformationHandler gameInformationHandler)
        {
            _gameInformationHandler = gameInformationHandler;
            Generation = 0;
            Species = new List<Specie>();
            CurrentFrame = 0;
            Timeout = Config.Timeout;
            RightmostPosition = 0;
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

            Species.Add(initSpecie);
            CurrentSpecie = initSpecie;
            CurrentPlayer = CurrentSpecie.Genomes.First();
        }

        public void Train()
        {
            if (CurrentFrame % 5 == 0)
            {
                EvaluateCurrentPlayer();
            }

            var marioX = _gameInformationHandler.MarioX;
            if (marioX > RightmostPosition)
            {
                RightmostPosition = marioX;
                Timeout = Config.Timeout;
            }

            Timeout -= 1;
            var timeoutBonus = CurrentFrame / 4;

            if (Timeout + timeoutBonus <= 0)
            {
                var fitness = RightmostPosition - CurrentFrame / 2;
                if (RightmostPosition > 3186)
                {
                    fitness += 1000;
                }

                CurrentPlayer.Fitness = fitness;
                Console.WriteLine($"Generation: {Generation}; Specie: {CurrentSpecie.Name} ({Species.IndexOf(CurrentSpecie) + 1}/{Species.Count}); Genome: {CurrentSpecie.Genomes.IndexOf(CurrentPlayer) + 1}/{CurrentSpecie.Genomes.Count}; Fitness: {fitness};");
                InitializeNextRun();
            }

            CurrentFrame++;
        }

        private void InitializeNextRun()
        {
            _gameInformationHandler.ClearJoyPad();
            _gameInformationHandler.LoadSaveState();
            RightmostPosition = 0;
            CurrentFrame = 0;
            Timeout = Config.Timeout;

            NextPlayer();
            EvaluateCurrentPlayer();
        }

        private void EvaluateCurrentPlayer()
        {
            var input = _gameInformationHandler.GetNeuralNetInputs();

            var output = CurrentPlayer.Propagate(input);

            _gameInformationHandler.HandleOutput(output);
        }

        private void NextGeneration()
        {
            var newGeneration = new List<Specie>();
            var newGenerationCount = 0;
            var totalAverageFitness = 0d;

            foreach (var specie in Species)
            {
                var childGenomes = specie.GetMostFitGenomes();
                if (childGenomes.Count > 0)
                {
                    var childSpecie = new Specie(specie.Name);
                    childSpecie.Genomes = childGenomes;
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
            species.Add(newSpecie);
        }

        private void NextPlayer()
        {
            var nextPlayerIndex = CurrentSpecie.Genomes.IndexOf(CurrentPlayer) + 1;
            if (nextPlayerIndex != CurrentSpecie.Genomes.Count)
            {
                CurrentPlayer = CurrentSpecie.Genomes[nextPlayerIndex];
                return;
            }

            var nextSpecieIndex = Species.IndexOf(CurrentSpecie) + 1;
            if (nextSpecieIndex != Species.Count)
            {
                CurrentSpecie = Species[nextSpecieIndex];
                CurrentPlayer = CurrentSpecie.Genomes.First();
                return;
            }

            NextGeneration();
            CurrentSpecie = Species.First();
            CurrentPlayer = CurrentSpecie.Genomes.First();
        }
    }
}
