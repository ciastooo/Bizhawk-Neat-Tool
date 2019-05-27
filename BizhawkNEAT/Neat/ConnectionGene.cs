using BizhawkNEAT.Utils;
using Newtonsoft.Json.Linq;

namespace BizhawkNEAT.Neat
{
    public class ConnectionGene
    {
        public int PreviousNodeId { get; private set; }
        public int NextNodeId { get; private set; }
        public double Weight { get; private set; }
        public bool IsEnabled { get; private set; }

        public ConnectionGene(int previousNodeId, int nextNodeId)
        {
            PreviousNodeId = previousNodeId;
            NextNodeId = nextNodeId;
            Weight = RandomGenerator.NewWeight();
            IsEnabled = true;
        }

        public ConnectionGene(int previousNodeId, int nextNodeId, double weight, bool isEnabled)
        {
            PreviousNodeId = previousNodeId;
            NextNodeId = nextNodeId;
            Weight = weight;
            IsEnabled = isEnabled;
        }

        public ConnectionGene(int previousNodeId, int nextNodeId, double weight)
        {
            PreviousNodeId = previousNodeId;
            NextNodeId = nextNodeId;
            Weight = weight;
            IsEnabled = true;
        }

        public ConnectionGene(ConnectionGene source)
        {
            PreviousNodeId = source.PreviousNodeId;
            NextNodeId = source.NextNodeId;
            Weight = source.Weight;
            IsEnabled = source.IsEnabled;
        }

        public ConnectionGene(JToken json)
        {
            PreviousNodeId = json.Value<int>("PreviousNodeId");
            NextNodeId = json.Value<int>("NextNodeId");
            Weight = json.Value<double>("Weight");
            IsEnabled = json.Value<bool>("IsEnabled");
        }


        public void SetWeight(double newWeight)
        {
            Weight = newWeight;
        }

        public void Toggle(bool value)
        {
            IsEnabled = value;
        }
    }
}
