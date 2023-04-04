using System;

namespace EasyState.Models.DataType.DataTypeConditions
{
    public class GreaterThanCondition<T, TProperty> : ComparisonDataTypeCondition<T, TProperty> where TProperty : IComparable<TProperty>, IComparable where T : DataTypeBase
    {
        public GreaterThanCondition(string name, Func<T, TProperty> valueFunc, Func<T, TProperty> valToCompareFunc, DataTypeConditionSet<T> set) : base(name, valueFunc, valToCompareFunc, set)
        {
        }

        public GreaterThanCondition(string name, Func<T, TProperty> valueFunc, TProperty valToCompare, DataTypeConditionSet<T> set) : base(name, valueFunc, valToCompare, set)
        {
        }

        protected override bool EvaluateComparison(int result) => result > 0;
    }
}