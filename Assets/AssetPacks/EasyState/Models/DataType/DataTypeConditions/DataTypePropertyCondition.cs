using EasyState.Models.DataType;
using System;

namespace EasyState.Models
{
    public abstract class DataTypePropertyCondition<T, TProperty> : DataTypeFunction<T>, IDataTypeCondition<T>, IConditionBuilder<T, TProperty> where T : DataTypeBase
    {
        public override string Id => $"{typeof(T).Name}-condition-{Name}";
        public DataTypeConditionSet<T> Set { get; }
        public abstract bool ValidCondition { get; }
        public Func<T, TProperty> ValueFunction { get; }

        string IConditionBuilder<T, TProperty>.Name =>  Name;

        DataTypeConditionSet<T> IConditionBuilder<T, TProperty>.Set => Set;

        Func<T, TProperty> IConditionBuilder<T, TProperty>.ValueFunction => ValueFunction;

        protected DataTypePropertyCondition(string name, Func<T, TProperty> valueFunc, DataTypeConditionSet<T> set) : base(name)
        {
            ValueFunction = valueFunc ?? throw new ArgumentNullException(nameof(valueFunc));
            Set = set ?? throw new ArgumentNullException(nameof(set));
            Set.AttachCondition(this);
        }

        public void DettachFromSet() => Set.DettachCondition(this);

        public bool Evaluate(T context)
        {
            if (!ValidCondition)
            {
                throw new InvalidOperationException("Can not evaluate this condition because it is not set up correctly. Make sure condition has name, value function, and value to compare function");
            }
            return OnEvaluate(context);
        }

        protected abstract bool OnEvaluate(T context);
        public bool BaseEvaluate(DataTypeBase data) => Evaluate((T)data);

        void IConditionBuilder<T, TProperty>.DettachFromSet() => DettachFromSet();

    }
}