using System;

namespace EasyState.Models.DataType.DataTypeConditions
{
    public class IsFalseCondition<T> : DataTypePropertyCondition<T, bool> where T : DataTypeBase
    {
        public override bool ValidCondition => ValueFunction != null;

        public IsFalseCondition(string name, Func<T, bool> valueFunc, DataTypeConditionSet<T> set) : base(name, valueFunc, set)
        {
        }

        protected override bool OnEvaluate(T context) => !ValueFunction.Invoke(context);
    }
}