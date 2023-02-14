using Newtonsoft.Json;

namespace EasyState.DataModels
{
    public class NodeDataShort
    {
        [JsonProperty("y")]
        public string Id { get; set; }
        [JsonProperty("z")]
        public string Name { get; set; }
        public NodeDataShort()
        {

        }
        public NodeDataShort(string id, string name)
        {
            Id = id;
            Name = name;
        }
        public NodeDataShort(NodeData node)
        {
            Id = node.Id;
            Name = node.Name;
        }
    }
}
