﻿using BizhawkNEAT.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace BizhawkNEAT.Neat
{
    public class Network
    {
        private readonly GameInformationHandler _gameInformationHandler;
        private readonly Graphics _networkGraphGraphics;

        public IList<Specie> Species { get; set; }

        public int Generation { get; set; }

        private int CurrentFrame { get; set; }
        private int Timeout { get; set; }
        private int RightmostPosition { get; set; }
        private Specie CurrentSpecie { get; set; }
        private Genome CurrentPlayer { get; set; }
        private bool[] CurrentOutput { get; set; }
        public int TopFitnessInGeneration { get; set; }

        public Network(GameInformationHandler gameInformationHandler, Graphics networkGraphGraphics)
        {
            _gameInformationHandler = gameInformationHandler;
            _networkGraphGraphics = networkGraphGraphics;
            Generation = 1;
            Species = new List<Specie>();
            CurrentFrame = 0;
            Timeout = Config.Timeout;
            RightmostPosition = 0;
        }

        public void Init(int inputNodesCount, int outputNodesCount)
        {
            var initSpecie = new Specie();

            var progenitorGenome = new Genome();
            for (int j = 0; j < inputNodesCount; j++)
            {
                var inputNode = new NodeGene(NodeGeneType.Input);
                progenitorGenome.AddNodeGene(inputNode, inputNode.Id);
            }
            for (int j = 0; j < outputNodesCount; j++)
            {
                var outputNode = new NodeGene(NodeGeneType.Output);
                progenitorGenome.AddNodeGene(outputNode, outputNode.Id);
            }

            for (int i = 0; i < Config.Population; i++)
            {
                var newGenome = new Genome(progenitorGenome);
                newGenome.TryMutate();
                initSpecie.Genomes.Add(newGenome);
            }

            Species.Add(initSpecie);
            CurrentSpecie = Species.First();
            CurrentPlayer = CurrentSpecie.Genomes.First();
        }

        public void Train()
        {
            if (CurrentFrame % 5 == 0)
            {
                EvaluateCurrentPlayer();
                if (Config.DrawGenome && CurrentFrame % 25 == 0)
                    DrawGenome();
            }

            _gameInformationHandler.HandleOutput(CurrentOutput);

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
                if (fitness > TopFitnessInGeneration)
                {
                    TopFitnessInGeneration = fitness;
                }
                Console.WriteLine($"Generation: {Generation}; Specie: {CurrentSpecie.Name} ({Species.IndexOf(CurrentSpecie) + 1}/{Species.Count}); Genome: {CurrentSpecie.Genomes.IndexOf(CurrentPlayer) + 1}/{CurrentSpecie.Genomes.Count}; Fitness: {fitness};");
                InitializeNextRun();
            }

            CurrentFrame++;
        }

        private void InitializeNextRun()
        {
            CurrentOutput = new bool[] { false, false, false, false, false, false };
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

            CurrentOutput = CurrentPlayer.Propagate(input);
        }

        private List<Specie> RemoveStaleSpecies(IList<Specie> species)
        {
            var survivedSpecies = new List<Specie>();
            foreach (var specie in species)
            {
                if (specie.Staleness < Config.StalenessThreshold || specie.TopFitness >= TopFitnessInGeneration)
                {
                    specie.Staleness = 0;
                    survivedSpecies.Add(specie);
                }
            }
            return survivedSpecies;
        }

        private IList<Specie> CullSpecies(IList<Specie> species)
        {
            foreach(var specie in species)
            {
                specie.Genomes = new List<Genome> { specie.Genomes.First() };
            }
            return species;
        }

        private void NextGeneration()
        {
            foreach (var specie in Species)
            {
                specie.Genomes = specie.GetMostFitGenomes();
            }

            Species = RemoveStaleSpecies(Species);

            var totalAverageFitness = Species.Sum(s => s.AverageFitness + 1);
            Species = Species.Where(s => (s.AverageFitness / totalAverageFitness) * Config.Population >= 1).ToList();
            totalAverageFitness = Species.Sum(s => s.AverageFitness + 1);

            var nextGenerationChildrenGenomes = new List<Genome>();
            foreach (var specie in Species)
            {
                var childrenGenomesTobreed = (int)((specie.AverageFitness / totalAverageFitness) * Config.Population - 1);
                for (int i = 0; i < childrenGenomesTobreed; i++)
                {
                    nextGenerationChildrenGenomes.Add(specie.Breed());
                }
            }

            Species = CullSpecies(Species);

            while (Species.Count + nextGenerationChildrenGenomes.Count < Config.Population)
            {
                var randomSpecie = Species.GetRandomElement();
                nextGenerationChildrenGenomes.Add(randomSpecie.Breed());
            }

            foreach (var childrenGenome in nextGenerationChildrenGenomes)
            {
                AssignSpecie(Species, childrenGenome);
            }

            Generation++;
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
            DrawingHelper.ClearCache();
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

        private void DrawGenome()
        {
            DrawingHelper.DrawGenome(_networkGraphGraphics, CurrentPlayer);
        }
    }
}
