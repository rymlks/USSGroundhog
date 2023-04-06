using System;

namespace EasyState.Models.DataType.DataTypeConditions
{
    /// <summary>
    /// This represents an on configured condition
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TProperty"></typeparam>
    public class InvalidInitialCondition<T, TProperty> : DataTypePropertyCondition<T, TProperty> where T : DataTypeBase
    {
        public override bool ValidCondition => false;

        public InvalidInitialCondition(string name, Func<T, TProperty> valueFunc, DataTypeConditionSet<T> set) : base(name, valueFunc, set)
        {
        }

        protected override bool OnEvaluate(T context)
        {
            throw new NotImplementedException();
        }
    }
    public class SimpleCondition<T> : DataTypePropertyCondition<T, bool> where T : DataTypeBase
    {
        public SimpleCondition(string name, Func<T, bool> valueFunc, DataTypeConditionSet<T> set) : base(name, valueFunc, set)
        {
        }

        public override bool ValidCondition => true;

        protected override bool OnEvaluate(T context)=>  ValueFunction.Invoke(context);
    }
}