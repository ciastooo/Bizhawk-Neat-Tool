using System;

namespace BizhawkNEAT.Utils
{
    public static class RandomGenerator
    {
        private static Random _instance { get; set; } = new Random();

        public static Random GetRandom()
        {
            return _instance;
        }

        public static double NewWeight(double limit = 1)
        {
            return GetRandom().NextDouble() * 2 * limit - limit;
        }

        public static bool GetRandomResult(double probability)
        {
            return GetRandom().NextDouble() < probability;
        }

        public static bool GetCoinToss()
        {
            return GetRandom().Next(0, 2) == 1;
        }
    }
}
