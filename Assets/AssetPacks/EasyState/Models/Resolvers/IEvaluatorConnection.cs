namespace EasyState.Models.Resolvers
{
    public interface IEvaluatorConnection<T> where T : DataTypeBase
    {
        State<T> DestState { get; }
        IEvaluator Evaluator { get; }
    }

}
