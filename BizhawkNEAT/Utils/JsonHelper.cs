using BizhawkNEAT.Neat;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BizhawkNEAT.Utils
{
    public static class JsonHelper
    {
        public static JToken GetNetworkJson(this Network network)
        {
            var json = new JObject();
            json.Add("Generation", network.Generation);

            json.Add("Species", JToken.FromObject(network.Species.Select(s => s.GetSpecieJson()).ToList()));

            return json;
        }

        public static JToken GetSpecieJson(this Specie specie)
        {
            var json = new JObject();

            json.Add("Name", specie.Name);
            json.Add("Staleness", specie.Staleness);
            json.Add("Genomes", JToken.FromObject(specie.Genomes.Select(g => g.GetGenomeJson()).ToList()));

            return json;
        }

        public static JToken GetGenomeJson(this Genome genome)
        {
            var json = new JObject();

            json.Add("ConnectionGenes", JToken.FromObject(
                genome.ConnectionGenes.Select(cg => new
                {
                    Key = cg.Key,
                    Value = cg.Value.GetConnectionGeneJson()
                }))
            );
            json.Add("NodeGenes", JToken.FromObject(
                genome.NodeGenes.Select(ng => ng.Value.GetNodeGeneJson()))
            );

            return json;
        }

        public static JToken GetConnectionGeneJson(this ConnectionGene connectionGene)
        {
            var json = new JObject();

            json.Add("PreviousNodeId", connectionGene.PreviousNodeId);
            json.Add("NextNodeId", connectionGene.NextNodeId);
            json.Add("Weight", connectionGene.Weight);
            json.Add("IsEnabled", connectionGene.IsEnabled);

            return json;
        }

        public static JToken GetNodeGeneJson(this NodeGene nodeGene)
        {
            var json = new JObject();

            json.Add("Id", nodeGene.Id);
            json.Add("Type", (int)nodeGene.Type);

            return json;
        }
    }
}
