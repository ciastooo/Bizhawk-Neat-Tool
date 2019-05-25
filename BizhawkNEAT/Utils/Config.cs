using System;
using System.Collections.Generic;

namespace BizhawkNEAT.Utils
{
    public static class Config
    {
        // Species distance coefficients
        public static double DisjointCoefficient { get; private set; } = 1;
        public static double WeightCoefficient { get; private set; } = 0.4;
        public static double SpecieThreshold { get; private set; } = 3;

        // Mutations
        public static double MutationAddConnectionProbability = 0.5;
        public static double MutationAddNodeProbability = 0.1;
        public static double MutationEnableConnectionProbability = 0.1;
        public static double MutationDisableConnectionProbability = 0.2;
        public static double MutationAdjustWeightProbability = 0.9;
        public static double MutationPerturbateWeightProbability = 0.1;
        public static double WeightStep { get; private set; } = 0.1;

        public static int Population { get; private set; } = 300;

        public static double CrossoverChance { get; private set; } = 0.75;

        public static int Timeout { get; private set; } = 20;

        private static IList<string> SpecieNames = new List<string>
        {
            "Ironman", "Captain America", "Black Widow", "Hulk", "Dr. Strange", "Thor", "Thanos", "Antman", "Black Panther", "Spiderman", "Wolverine", "Falcon", "Drax", "Rocket", "Hawkeye", "War machine", "Star-lord", "The Winter Soldier", "Wasp", "Groot", "Vision", "Loki", "Stan Lee"
        };

        public static string GetNewSpecieName()
        {
            if (SpecieNames.Count == 0)
            {
                return Guid.NewGuid().ToString();
            }
            var randomName = SpecieNames.GetRandomElement();
            SpecieNames.Remove(randomName);
            return randomName;
        }

        public static string[] ButtonNames = new string[] { "A", "B", "Up", "Down", "left", "Right" };
    }
}
