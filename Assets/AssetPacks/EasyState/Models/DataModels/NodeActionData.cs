using Newtonsoft.Json;

namespace EasyState.DataModels
{
    public class NodeActionData : ModelData
    {
        [JsonProperty("e")]
        public string ActionID { get; set; }

        [JsonProperty("d")]
        public string ConditionID { get; set; }

        [JsonProperty("a")]
        public bool IsConditional { get; set; }

        [JsonProperty("c")]
        public string NodeID { get; set; }

        [JsonProperty("b")]
        public int Priority { get; set; }

        [JsonProperty("f")]
        public bool ExecuteWhenFalse { get; set; }
        [JsonProperty("g")]
        public NodeActionExecutionPhase ActionPhase { get; set; }
        [JsonProperty("h")]
        public string ParameterDataString { get; set; }
    }
}