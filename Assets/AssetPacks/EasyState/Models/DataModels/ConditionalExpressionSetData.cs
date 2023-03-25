using Newtonsoft.Json;
using System.Collections.Generic;

namespace EasyState.DataModels
{
    public class ConditionalExpressionSetData : ModelData
    {
        [JsonProperty("b")]
        public List<ConditionalExpressionRowData> AdditionalRows { get; set; } = new List<ConditionalExpressionRowData>();

        [JsonProperty("a")]
        public ConditionalExpressionRowData InitialConditionalRow { get; set; }

    }
}