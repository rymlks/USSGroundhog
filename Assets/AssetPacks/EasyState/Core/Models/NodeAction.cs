using EasyState.DataModels;

namespace EasyState.Core.Models
{
    public class NodeAction : Model, IDataModel<NodeActionData>
    {
        public FunctionModel Action { get; set; }
        public FunctionModel Condition { get; set; }
        public bool ExecuteWhenFalse { get; set; }
        public bool IsConditional { get; set; }
        public string NodeID { get; set; }
        public int Priority { get; set; }
        public NodeActionExecutionPhase ActionPhase { get; set; }
        public string ParameterDataString { get; set; }
        public NodeAction(NodeActionData data) : base(data)
        {
            IsConditional = data.IsConditional;
            Priority = data.Priority;
            NodeID = data.NodeID;
            ExecuteWhenFalse = data.ExecuteWhenFalse;
            ActionPhase = data.ActionPhase;
            ParameterDataString = data.ParameterDataString; 

        }

        public NodeAction(string nodeID, string id = null) : base(id)
        {
            NodeID = nodeID;
        }

        public NodeActionData Serialize()
        {
            var data = new NodeActionData()
            {

                IsConditional = IsConditional,
                Priority = Priority,
                NodeID = NodeID,
                ConditionID = Condition?.Id,
                ActionID = Action?.Id,
                ExecuteWhenFalse = ExecuteWhenFalse,
                ActionPhase = ActionPhase,
                ParameterDataString = ParameterDataString,
            };
            data.SetModelData(this);
            return data;
        }
    }
}