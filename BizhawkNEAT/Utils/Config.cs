﻿using System.Collections.Generic;

namespace BizhawkNEAT.Utils
{
    public static class Config
    {
        // Species distance coefficients
        public static double DisjointDelta { get; private set; } = 2;
        public static double WeightDelta { get; private set; } = 0.4;
        public static int SpecieSizeDelta { get; private set; } = 20;
        public static double SpecieThreshold { get; private set; }

        // Mutations
        public static double MutationAddConnectionProbability = 0.5;
        public static double MutationAddNodeProbability = 0.1;
        public static double MutationEnableConnectionProbability = 0.1;
        public static double MutationDisableConnectionProbability = 0.2;
        public static double MutationAdjustWeightProbability = 0.9;
        public static double MutationPerturbateWeightProbability = 0.1;
        public static double MutationDeleteConnectionProbability = 0.15;
        public static double WeightStep { get; private set; } = 0.1;

        public static int Population { get; private set; } = 100;

        public static double CrossoverChance { get; private set; } = 0.75;

        private static IList<string> SpecieNames = new List<string>
        {
            "Ironman", "Captain America", "Black Widow", "Hulk", "Dr. Strange", "Thor", "Thanos", "Antman", "Black Panther", "Spiderman", "Wolverine", "Falcon", "Drax", "Rocket", "Hawkeye", "War machine", "Star-lord", "The Winter Soldier", "Wasp", "Groot", "Vision", "Loki", "Stan Lee"
        };

        public static string GetNewSpecieName()
        {
            var randomName = SpecieNames.GetRandomElement();
            SpecieNames.Remove(randomName);
            return randomName;
        }
    }
}
