namespace EasyState.Models
{
    public abstract class Evaluator<T> : FunctionBase where T : DataTypeBase
    {
        public abstract float GetScore(T data);
    }
}