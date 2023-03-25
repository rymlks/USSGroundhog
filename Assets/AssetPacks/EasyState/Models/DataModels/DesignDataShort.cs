using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace EasyState.DataModels
{
    public class DesignDataShort:ModelData
    {
        [JsonProperty("c")]
        public string DataTypeID { get; set; }
        [JsonProperty("d")]
        public string DataTypeName { get; set; }
        [JsonProperty("e")]
        public List<NodeDataShort> Nodes { get; set; } = new List<NodeDataShort>();
        [JsonProperty("f")]
        public int ConnectionCount { get; set; }

        public DesignDataShort()
        {

        }
       
        public DesignDataShort(DesignData data, DataTypeModel dataType)
        {
            Id = data.Id;
            Name = data.Name;
            ConnectionCount = data.Nodes.Sum(x => x.Connections.Count);
            Nodes = data.Nodes.Select(x => new NodeDataShort(x)).ToList();
            DataTypeName = dataType.Name;
            DataTypeID = dataType.Id;


        }
    }
}
