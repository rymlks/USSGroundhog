using System;

namespace EasyState.Models.DataType.DataTypeEvaluators
{
    public class CustomEvaluator<T> : DataTypeFunction<T>,  IDataTypeEvaluator<T> where T : DataTypeBase
    {
        public override string Id => $"{typeof(T).Name}-evaluator-{Name}";
        private readonly Func<T, float> _evalFunc;

        public CustomEvaluator(string name, Func<T, float> evalFunc) :base(name)
        {           
            if (evalFunc is null)
            {
                throw new ArgumentNullException("evalFunc");
            }            
            _evalFunc = evalFunc;
        }
        public float GetScore(T data)=> _evalFunc.Invoke(data);

        public float BaseGetScore(DataTypeBase data)=> _evalFunc.Invoke((T)data);
    }
}