using Newtonsoft.Json;
using System.Collections.Generic;

namespace EasyState.DataModels
{
    public class ConditionalExpressionRowData : ModelData
    {
        [JsonProperty("b")]
        public List<ConditionalExpressionData> AdditionalExpressions { get; set; } = new List<ConditionalExpressionData>();

        [JsonProperty("c")]
        public ConditionalLogicType ConditionalLogicType { get; set; }

        [JsonProperty("a")]
        public ConditionalExpressionData InitialExpressionData { get; set; }   
    }
}