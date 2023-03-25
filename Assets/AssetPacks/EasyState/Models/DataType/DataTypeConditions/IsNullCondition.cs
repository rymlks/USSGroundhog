using System;

namespace EasyState.Models.DataType.DataTypeConditions
{
    public class IsNullCondition<T, TProperty> : DataTypePropertyCondition<T, TProperty> where T : DataTypeBase where TProperty : class
    {
        public override bool ValidCondition => ValueFunction != null;

        public IsNullCondition(string name, Func<T, TProperty> valueFunc, DataTypeConditionSet<T> set) : base(name, valueFunc, set)
        {
        }

        protected override bool OnEvaluate(T context) => ValueFunction.Invoke(context) == null;
    }
}