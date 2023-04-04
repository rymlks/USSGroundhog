using System;

namespace EasyState.Models
{
    public interface IConditionBuilder<T, TProperty> where T : DataTypeBase
    {
        string Name { get; }
        DataTypeConditionSet<T> Set { get; }
        Func<T, TProperty> ValueFunction { get; }
        void DettachFromSet();
    }
}