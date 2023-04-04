using Newtonsoft.Json;

namespace EasyState.DataModels
{
    public class ConditionalExpressionData : ModelData
    {
        [JsonProperty("c")]
        public ConditionalLogicType ConditionalLogicType { get; set; }

        [JsonProperty("a")]
        public string ConditionID { get; set; }

        [JsonProperty("b")]
        public bool ExpectedResult { get; set; }
    }
}