namespace EasyState.Models.Resolvers
{
    public class ConditionConnection<T> : IConditionConnection<T> where T : DataTypeBase
    {
        public State<T> DestState { get; }

        public ICondition<T> Condition { get; }
        public ConditionConnection(State<T> destState, ICondition<T> condition)
        {
            DestState = destState;
            Condition = condition;
        }

    }
}
