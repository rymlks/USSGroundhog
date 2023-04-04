using EasyState.Models.DataType;
using EasyState.Models.DataType.DataTypeConditions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EasyState.Models
{
    public class DataTypeConditionSet<T> : IDataTypeFunctionSubset<T> where T : DataTypeBase
    {
        public IReadOnlyList<IDataTypeCondition<T>> Conditions => _conditions;

        public IReadOnlyList<IDataTypeFunction<T>> GenericSet => _conditions.Select(x=> x.AsFunction()).ToList();

        public IReadOnlyList<IDataTypeFunction> Set => GenericSet;

        private readonly List<IDataTypeCondition<T>> _conditions = new List<IDataTypeCondition<T>>();

        public IConditionBuilder<T, TProperty> AddCondition<TProperty>(Func<T, TProperty> valueFunc, string name)
        {
            if(typeof(TProperty) == typeof(bool))
            {
                var convertedValueFunc = (Func<T, bool>)((object)valueFunc);
                return  new SimpleCondition<T>(name, convertedValueFunc, this) as DataTypePropertyCondition<T, TProperty>;
            }
            return new InvalidInitialCondition<T, TProperty>(name, valueFunc, this);
        } 

        public IDataTypeCondition<T> AddCustomCondition(string name, Func<T, bool> condition) => new CustomCondition<T>(name, condition, this);

        public void AttachCondition(IDataTypeCondition<T> condition)
        {
            if (GenericSet.Any(x => x.Name == condition.AsFunction().Name))
            {
                Debug.LogError($"Can not add condition named {condition.AsFunction().Name}, because a condition of that name already exists in the set.");
                return;
            }
            _conditions.Add(condition);
        }
        public bool DettachCondition(IDataTypeCondition<T> condition) => _conditions.Remove(condition);

    }
}