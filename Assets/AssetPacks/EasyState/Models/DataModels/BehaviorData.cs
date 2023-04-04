using Newtonsoft.Json;

namespace EasyState.DataModels
{
    [System.Serializable]
    public class BehaviorData
    {
        [JsonProperty("a")]
        public string DataTypeId;
        [JsonProperty("b")]
        public string DataTypeFullName;
        [JsonProperty("c")]
        public string DesignId;
        [JsonProperty("d")]
        public string BehaviorName;

    }
}
