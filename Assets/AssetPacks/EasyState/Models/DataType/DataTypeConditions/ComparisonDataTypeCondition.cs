using System;

namespace EasyState.Models
{
    public abstract class ComparisonDataTypeCondition<T, TProperty> : DataTypePropertyCondition<T, TProperty> where TProperty : IComparable<TProperty>, IComparable where T : DataTypeBase
    {
        public override bool ValidCondition => !string.IsNullOrEmpty(Name) && ValueFunction != null && (ValueToCompareFunction != null || ValueToCompare != null);
        public TProperty ValueToCompare { get; set; }
        public Func<T, TProperty> ValueToCompareFunction { get; }

        protected ComparisonDataTypeCondition(string name, Func<T, TProperty> valueFunc, Func<T, TProperty> valToCompareFunc, DataTypeConditionSet<T> set) : base(name, valueFunc, set)
        {
            ValueToCompareFunction = valToCompareFunc;
        }

        protected ComparisonDataTypeCondition(string name, Func<T, TProperty> valueFunc, TProperty valToCompare, DataTypeConditionSet<T> set) : base(name, valueFunc, set)
        {
            ValueToCompare = valToCompare;
        }

        protected abstract bool EvaluateComparison(int result);

        protected override bool OnEvaluate(T context)
        {
            TProperty thisValue = ValueFunction.Invoke(context);
            TProperty valueToCompare;
            if (ValueToCompareFunction != null)
            {
                valueToCompare = ValueToCompareFunction.Invoke(context);
            }
            else
            {
                valueToCompare = ValueToCompare;
            }

            if (thisValue == null)
            {
                return false;
            }
            return EvaluateComparison(thisValue.CompareTo(valueToCompare));
        }
    }
}