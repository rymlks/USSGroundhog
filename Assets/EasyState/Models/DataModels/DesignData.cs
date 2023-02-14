using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EasyState.DataModels
{
    public class DesignData : ModelData
    {
        [JsonProperty("a")]
        public string DataTypeID { get; set; }

        [JsonProperty("f")]
        public List<GroupData> Groups { get; set; } = new List<GroupData>();

        [JsonProperty("d")]
        public List<NodeData> Nodes { get; set; } = new List<NodeData>();

        [JsonProperty("g")]
        public List<NoteData> Notes { get; set; } = new List<NoteData>();

        [JsonProperty("b")]
        public Vector2Data Pan { get; set; }

        [JsonProperty("c")]
        public float Zoom { get; set; }

        //Used by Newtonsoft
        public DesignData()
        {
        }

        public DesignData(string dataTypeID, string designID = null)
        {
            DataTypeID = dataTypeID;
            Id = Guid.NewGuid().ToString();
        }
        public bool DataEqualTo(DesignData otherData)
        {
            string thisJson = JsonConvert.SerializeObject(this);
            string otherJson = JsonConvert.SerializeObject(otherData);
            return thisJson.Equals(otherJson);
        }
    }
}