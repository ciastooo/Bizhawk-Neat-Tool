namespace BizhawkNEAT.Utils
{
    public static class IdGenerator
    {
        private static int _current { get; set; } = 0;

        public static void Reset(int newValue = 0)
        {
            _current = newValue;
        }

        public static int Next()
        {
            return _current++;
        }
    }
}
