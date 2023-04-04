namespace EasyState.Models
{
    public interface ICondition
    {
        bool BaseEvaluate(DataTypeBase data);
    }
    public interface ICondition<T> : ICondition where T : DataTypeBase
    {
        bool Evaluate(T data);
    } 
}