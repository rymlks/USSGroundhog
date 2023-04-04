namespace EasyState.Models
{
    public interface IEvaluator
    {
        float BaseGetScore(DataTypeBase data);
    }
    public interface IEvaluator<T> : IEvaluator where T : DataTypeBase
    {
        float GetScore(T data);
    }
}