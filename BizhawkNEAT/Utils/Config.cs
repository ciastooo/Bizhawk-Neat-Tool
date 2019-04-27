namespace BizhawkNEAT.Utils
{
    public static class Config
    {
        // Species distance coefficients
        public static double DisjointDelta { get; private set; } = 2d;
        public static double WeightDelta { get; private set; } = 0.4d;
        public static int SpecieSizeDelta { get; private set; } = 20;
        public static double SpecieThreshold { get; private set; }

        public static double Step { get; private set; }

        public static int Population { get; private set; } = 500;

        public static void Set(double disjointDelta, double weightDelta, int specieSizeDelta, double specieThreshold, double step)
        {
            DisjointDelta = disjointDelta;
            WeightDelta = weightDelta;
            SpecieSizeDelta = specieSizeDelta;
            SpecieThreshold = specieThreshold;
            Step = step;
        }
    }
}
