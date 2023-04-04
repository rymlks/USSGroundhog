using EasyState.Data;
using EasyState.DataModels;
using EasyState.Models;
using EasyState.Models.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
namespace EasyState.Cache
{


    public class FunctionCacheSet<T> where T : DataTypeBase
    {

        private readonly DataTypeModel _dataType;
        private readonly List<IDataTypeFunction> _nonScriptFunctions = new List<IDataTypeFunction>();
        public FunctionCacheSet(T instance, DataTypeModel dataType)
        {
            _dataType = dataType;
            var sets = dataType.GetAllDataTypeFunctionSets(instance);

            foreach (var set in sets)
            {
                _nonScriptFunctions.AddRange(set.Actions.Set);
                _nonScriptFunctions.AddRange(set.Conditions.Set);
                _nonScriptFunctions.AddRange(set.Evaluators.Set);
            }

        }
        public IAction GetAction(string id, string parameterData)
        {
            var actionData = _dataType.Functions.First(x => x.Id == id);
            if (actionData.HasScriptFile)
            {
                if (actionData.HasParameter)
                {
                    return GetParameterizedAction(actionData,parameterData);
                }
                else
                {
                    return GetScriptAction(actionData);
                }
            }
            else
            {
                return GetDataTypeAction(id);
            }
        }
        public ICondition GetCondition(string id)
        {
            var conditionData = _dataType.Functions.First(x => x.Id == id);
            if (conditionData.HasScriptFile)
            {
                return GetScriptCondition(conditionData);
            }
            else
            {
                return GetDataTypeCondition(id);
            }
        }
        public IEvaluator GetEvaluator(string id)
        {
            var evaluatorData = _dataType.Functions.First(x => x.Id == id);
            if (evaluatorData.HasScriptFile)
            {
                return GetScriptEvaluator(evaluatorData);
            }
            else
            {
                return GetDataTypeEvaluator(id);
            }
        }

        private IAction GetDataTypeAction(string id) => _nonScriptFunctions.First(x => x.Id == id && x is IAction) as IAction;
        private ICondition GetDataTypeCondition(string id) => _nonScriptFunctions.First(x => x.Id == id && x is ICondition) as ICondition;
        private IEvaluator GetDataTypeEvaluator(string id) => _nonScriptFunctions.First(x => x.Id == id && x is IEvaluator) as IEvaluator;
        private IAction GetScriptAction(FunctionModel actionData)
        {
            var action = TypePoolCache.DefaultCache.GetObjectInstance(actionData.AssemblyQualifiedName);
            return (action as IAction);
        }
        private IAction GetParameterizedAction(FunctionModel actionData, string parameterAsString)
        {
            var action = Activator.CreateInstance(Type.GetType(actionData.AssemblyQualifiedName),actionData.ParseValue(parameterAsString));
            return action as IAction;
        }
        private ICondition GetScriptCondition(FunctionModel conditionData)
        {
            var condition = TypePoolCache.DefaultCache.GetObjectInstance(conditionData.AssemblyQualifiedName);
            return (condition as ICondition);
        }
        private IEvaluator GetScriptEvaluator(FunctionModel evaluatorData)
        {
            var evaluator = TypePoolCache.DefaultCache.GetObjectInstance(evaluatorData.AssemblyQualifiedName);
            return (evaluator as IEvaluator);
        }
    }

}