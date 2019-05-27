using BizhawkNEAT.Utils;
using Newtonsoft.Json.Linq;

namespace BizhawkNEAT.Neat
{
    public class NodeGene
    {
        public int Id { get; set; }
        public NodeGeneType Type { get; private set; }

        public double Value { get; set; }

        public bool IsReady { get; set; }

        public NodeGene(NodeGeneType type = NodeGeneType.Hidden)
        {
            Id = IdGenerator.NextNodeId();
            Type = type;
            Value = 0;
            IsReady = false;
        }

        public NodeGene(NodeGene source)
        {
            Id = source.Id;
            Type = source.Type;
            Value = source.Value;
            IsReady = source.IsReady;
        }

        public NodeGene(JToken json)
        {
            Id = json.Value<int>("Id");
            Type = (NodeGeneType)json.Value<int>("Type");
            Value = 0;
            IsReady = false;
        }
    }
}
