namespace BizhawkNEAT.Utils
{
    public static class IdGenerator
    {
        public static int ConnectionCounter { get; private set; } = 0;
        public static int NodeCounter { get; private set; } = 0;

        public static void Reset(int value = 0)
        {
            ConnectionCounter = value;
            NodeCounter = value;
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
