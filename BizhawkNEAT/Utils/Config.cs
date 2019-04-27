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
        public static double WeightStep { get; private set; }

        public static int Population { get; private set; } = 100;

        public static void Set(
            double disjointDelta,
            double weightDelta,
            int specieSizeDelta,
            double specieThreshold,
            double mutationAddConnectionProbability,
            double mutationAddNodeProbability,
            double mutationEnableConnectionProbability,
            double mutationDisableConnectionProbability,
            double mutationAdjustWeightProbability,
            double mutationPerturbateWeightProbability,
            double mutationDeleteConnectionProbability,
            double step,
            int population)
        {
            DisjointDelta = disjointDelta;
            WeightDelta = weightDelta;
            SpecieSizeDelta = specieSizeDelta;
            SpecieThreshold = specieThreshold;
            MutationAddConnectionProbability = mutationAddConnectionProbability;
            MutationAddNodeProbability = mutationAddNodeProbability;
            MutationEnableConnectionProbability = mutationEnableConnectionProbability;
            MutationDisableConnectionProbability = mutationDisableConnectionProbability;
            MutationAdjustWeightProbability = mutationAdjustWeightProbability;
            MutationPerturbateWeightProbability = mutationPerturbateWeightProbability;
            MutationDeleteConnectionProbability = mutationDeleteConnectionProbability;
            Step = step;
            Population = population;
        }
    }
}
