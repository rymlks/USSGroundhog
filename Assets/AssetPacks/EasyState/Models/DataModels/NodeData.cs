using Newtonsoft.Json;
using System.Collections.Generic;

namespace EasyState.DataModels
{
    public class NodeData : MoveableData
    {
        [JsonProperty("a")]
        public NodeActionExecutionType ActionExecutionType { get; set; }

        [JsonProperty("b")]
        public NodeConditionType ConditionType { get; set; }

        [JsonProperty("c")]
        public List<ConnectionData> Connections { get; set; } = new List<ConnectionData>();

        [JsonProperty("d")]
        public NodeCycleType CycleType { get; set; }

        [JsonProperty("e")]
        public List<NodeActionData> NodeActions { get; set; } = new List<NodeActionData>();

        [JsonProperty("f")]
        public ColorData NodeColor { get; set; }

        [JsonProperty("g")]
        public ColorData SelectedNodeColor { get; set; }

        [JsonProperty("h")]
        public Vector2Data Size { get; set; }

        [JsonProperty("i")]
        public string JumpDesignID { get; set; }

        [JsonProperty("j")]
        public string JumpNodeID { get; set; }

        [JsonProperty("k")]
        public bool IsJumpNode { get; set; }

        [JsonProperty("l")]
        public bool IsEntryNode { get; set; }
        [JsonProperty("m")]
        public float ExitDelay { get; set; }
        [JsonProperty("n")]
        public string ExitDelayField { get; set; }
        [JsonProperty("o")]
        public string NodeSummary { get; set; }
        [JsonProperty("p")]
        public bool IsSummaryVisible { get; set; }


    }
}