namespace EasyState.Models.Resolvers
{
    public interface IConditionConnection<T> where T : DataTypeBase
    {
        State<T> DestState { get; }
        ICondition<T> Condition { get; }

    }
}
