namespace BizhawkNEAT.Utils
{
    public static class IdGenerator
    {
        private static int _connectionCounter { get; set; } = 0;
        private static int _nodeCounter { get; set; } = 0;

        public static void Reset(int value = 0)
        {
            _connectionCounter = value;
            _nodeCounter = value;
        }

        public static int NextConnectionId()
        {
            return _connectionCounter++;
        }

        public static int NextNodeId()
        {
            return _nodeCounter++;
        }
    }
}
