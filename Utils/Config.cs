namespace BizhawkNEAT.Utils
{
    public static class Config
    {
        public static double C1 { get; private set;}
        public static double C2 { get; private set;}
        public static double C3 { get; private set;}

        public static double Step { get; private set; }

        public static void Setup(double c1, double c2, double c3, double step)
        {
            C1 = c1;
            C2 = c2;
            C3 = c3;
            Step = step;
        }
    }
}
