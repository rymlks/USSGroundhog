using EasyState.Models.DataType;
using EasyState.Models.DataType.DataTypeEvaluators;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EasyState.Models
{
    public class DataTypeEvaluatorSet<T> : IDataTypeFunctionSubset<T> where T : DataTypeBase
    {
        public IReadOnlyList<IDataTypeEvaluator<T>> Evaluators => _evaluators;

        public IReadOnlyList<IDataTypeFunction<T>> GenericSet => _evaluators.Select(x=> x.AsFunction()).ToList();

        public IReadOnlyList<IDataTypeFunction> Set => GenericSet;

        private readonly List<IDataTypeEvaluator<T>> _evaluators = new List<IDataTypeEvaluator<T>>();

        public IDataTypeEvaluator<T> AddEvaluator(Func<T, float> evalFunc, string evaluatorName)
        {
            var newEvaluator = new CustomEvaluator<T>(evaluatorName, evalFunc);
            if (Evaluators.Any(x => x.AsFunction().Name == newEvaluator.Name))
            {
                Debug.LogError($"Can not add evaluator named {newEvaluator.Name}, because an evaluator of that name already exists in the set.");
                return null;
            }
            _evaluators.Add(newEvaluator);
            return newEvaluator;
        }

    }
}