using System;

namespace BizhawkNEAT.Helpers
{
    public static class RandomGenerator
    {
        private static Random _instance { get; set; }
        private static bool instantized { get; set; } = false;

        public static Random GetRandom()
        {
            if(!instantized)
            {
                _instance = new Random();
                instantized = true;
            }
            return _instance;
        }

        public static double NewWeight()
        {
            return GetRandom().NextDouble() * 2 - 1;
        }
    }
}
