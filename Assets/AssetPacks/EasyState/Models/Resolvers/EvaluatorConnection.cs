namespace EasyState.Models.Resolvers
{
    public class EvaluatorConnection<T> : IEvaluatorConnection<T> where T : DataTypeBase
    {
        public State<T> DestState { get; }

        public IEvaluator Evaluator { get; }
        public EvaluatorConnection(State<T> destState, IEvaluator evaluator)
        {
            DestState = destState;
            Evaluator = evaluator;
        }

    }
}
