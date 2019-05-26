namespace BizhawkNEAT.Utils
{
    public static class IdGenerator
    {
        public static int ConnectionCounter { get; private set; } = 0;
        public static int NodeCounter { get; private set; } = 0;

        public static void Reset(int connectionCounter = 0, int nodeCounter = 0)
        {
            ConnectionCounter = connectionCounter;
            NodeCounter = nodeCounter;
        }

        public static int NextConnectionId()
        {
            return ConnectionCounter++;
        }

        public static int NextNodeId()
        {
            return NodeCounter++;
        }

    }
}
