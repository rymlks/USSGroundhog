using Newtonsoft.Json;

namespace EasyState.DataModels
{
    public class ConnectionData : MoveableData
    {
        [JsonProperty("a")]
        public bool AutoPosition { get; set; }

        [JsonProperty("g")]
        public ConditionalExpressionSetData ConditionalExpression { get; set; }

        [JsonProperty("h")]
        public NodeConditionType ConnectionType { get; set; }

        [JsonProperty("c")]
        public string DestNodeID { get; set; }

        [JsonProperty("i")]
        public string EvaluatorID { get; set; }

        [JsonProperty("f")]
        public bool IsFallback { get; set; }

        [JsonProperty("e")]
        public int Priority { get; set; }

        [JsonProperty("d")]
        public Vector2Data Size { get; set; }

        [JsonProperty("b")]
        public string SourceNodeID { get; set; }
    }
}