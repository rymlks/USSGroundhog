namespace EasyState.Models.Resolvers
{
    public class FallbackConnection<T> : IConditionConnection<T> where T : DataTypeBase
    {
        public ICondition<T> Condition { get; }
        public State<T> DestState { get; }

        public FallbackConnection(State<T> destState)
        {
            DestState = destState;
            Condition = new AlwaysTrueCondition<T>();
        }
    }
}
