using System;
using System.Collections.Generic;

namespace BizhawkNEAT.Utils
{
    public static class Config
    {
        // Species distance coefficients
        public static double DisjointCoefficient { get; private set; } = 2;
        public static double WeightCoefficient { get; private set; } = 0.4;
        public static double SpecieThreshold { get; private set; } = 3.5;

        // Mutations
        public static double MutationAddConnectionProbability = 0.7;
        public static double MutationAddConnectionToBiasProbability = 0.3;
        public static double MutationAddNodeProbability = 0.3;
        public static double MutationEnableConnectionProbability = 0.2;
        public static double MutationDisableConnectionProbability = 0.3;
        public static double MutationAdjustWeightProbability = 0.25;
        public static double MutationPerturbateWeightProbability = 0.1;
        public static double WeightStep { get; private set; } = 0.1;

        public static int Population { get; private set; } = 300;
        public static int StalenessThreshold { get; private set; } = 15;
        public static double CrossoverChance { get; private set; } = 0.75;

        public static int Timeout { get; private set; } = 20;

        private static int SpecieId = 0;

        public static string GetNewSpecieName()
        {
            SpecieId++;
            return SpecieId.ToString();
        }

        public static string[] ButtonNames = new string[] { "A", "B", "Up", "Down", "Left", "Right" };

        // Form

        public static bool DrawGenome { get; set; } = false;

        public static int LeftXOffset { get; set; } = -1;
        public static int RightXOffset { get; set; } = 4;
        public static int UpYOffset { get; set; } = -2;
        public static int DownYOffset { get; set; } = 4;

        public static int InputNodesCount = (Math.Abs(LeftXOffset) + RightXOffset + 1) * (Math.Abs(UpYOffset) + DownYOffset + 1) + 1;
    }
}
