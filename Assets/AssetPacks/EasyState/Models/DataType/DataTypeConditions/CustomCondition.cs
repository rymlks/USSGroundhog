using System;

namespace EasyState.Models.DataType.DataTypeConditions
{
    public class CustomCondition<T, TProperty> : DataTypePropertyCondition<T, TProperty> where T : DataTypeBase
    {
        public override bool ValidCondition => _propertyvaluatorFunc != null || _contextValueFunc != null;
        private Func<T, TProperty, bool> _contextValueFunc;
        private Func<TProperty, bool> _propertyvaluatorFunc;

        public CustomCondition(string name, Func<T, TProperty> valueFunc, DataTypeConditionSet<T> set, Func<TProperty, bool> propertyEvaluatorFunc) : base(name, valueFunc, set)
        {
            _propertyvaluatorFunc = propertyEvaluatorFunc;
        }

        public CustomCondition(string name, Func<T, TProperty> valueFunc, DataTypeConditionSet<T> set, Func<T, TProperty, bool> contextEvaluatorFunc) : base(name, valueFunc, set)
        {
            _contextValueFunc = contextEvaluatorFunc;
        }

        protected override bool OnEvaluate(T context)
        {
            if (_propertyvaluatorFunc != null)
            {
                return _propertyvaluatorFunc.Invoke(ValueFunction.Invoke(context));
            }
            else
            {
                return _contextValueFunc.Invoke(context, ValueFunction.Invoke(context));
            }
        }
    }

    public class CustomCondition<T> : DataTypeFunction<T>, IDataTypeCondition<T> where T : DataTypeBase
    { 
        public DataTypeConditionSet<T> Set { get; }

        public override string Id => $"{typeof(T).Name}-condition-{Name}";

        private readonly Func<T, bool> _function;

        public CustomCondition(string name, Func<T, bool> evaluator, DataTypeConditionSet<T> set): base(name)
        {
            if (evaluator is null)
            {
                throw new ArgumentNullException(nameof(evaluator));
            }
            _function = evaluator;           
            Set = set;
            Set.AttachCondition(this);
        }

        public bool Evaluate(T context) => _function.Invoke(context);

        public bool BaseEvaluate(DataTypeBase data) => _function.Invoke((T)data);
    }
}