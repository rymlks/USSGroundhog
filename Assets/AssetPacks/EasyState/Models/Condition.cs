namespace EasyState.Models
{
    public abstract class Condition<T> : FunctionBase, ICondition<T> where T : DataTypeBase
    {
        public bool BaseEvaluate(DataTypeBase data) => Evaluate((T)data);

        public abstract bool Evaluate(T data);
    }
}