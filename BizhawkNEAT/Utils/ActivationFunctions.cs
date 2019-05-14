using System;

namespace BizhawkNEAT.Utils
{
    public static class ActivationFunctions
    {
        public static class Sigmoid
        {
            public static double Count(double x)
            {
                return 1 / (1 + Math.Exp(-x));
            }

            public static double Derivative(double x)
            {
                return x * (1 - x);
            }
        }
    }
}
