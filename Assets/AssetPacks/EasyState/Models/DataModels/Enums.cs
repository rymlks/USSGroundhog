namespace EasyState.DataModels
{
    public enum ConditionalLogicType { AND, OR }
    public enum NodeActionExecutionType { Default, ConditionalExecution }
    public enum NodeActionExecutionPhase { OnEnter,OnUpdate,OnExit }
    public enum NodeConditionType { Default, Repeat, Utility }
    public enum NodeCycleType { PassThrough, YieldCycle, YieldSeconds,YieldVariableDelay }
    public enum FunctionType { Action, Condition, Evaluator } 
}
