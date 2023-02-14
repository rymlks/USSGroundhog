using EasyState.Data;
using EasyState.DataModels;
using EasyState.Models.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace EasyState.Models
{
    public class DataTypeFunctionSet<T> : IFunctionModelSet, IDataTypeFunctionSet where T : DataTypeBase
    {
        private class CachedGetter
        {
            public object Getter;
            public string DisplayName;
        }
        private static class ReflectedConditionsCache
        {
            public static Dictionary<Type, List<CachedGetter>> FieldGetterCache = new Dictionary<Type, List<CachedGetter>>();
            public static Dictionary<Type, List<CachedGetter>> PropertyGetterCache = new Dictionary<Type, List<CachedGetter>>();

        }
        public DataTypeActionSet<T> Actions = new DataTypeActionSet<T>();
        public DataTypeConditionSet<T> Conditions = new DataTypeConditionSet<T>();
        public DataTypeEvaluatorSet<T> Evaluators = new DataTypeEvaluatorSet<T>();

        public DataTypeFunctionSet()
        {
            var dataType = typeof(T);

            LoadFieldConditions(dataType);

            if (ReflectedConditionsCache.PropertyGetterCache.TryGetValue(dataType, out List<CachedGetter> getterCache))
            {
                foreach (var getter in getterCache)
                {
                    Conditions.AddCondition(getter.Getter as Func<T, bool>, getter.DisplayName);
                }
            }
            else
            {
                var boolProps = dataType.GetProperties().Where(x => x.CanRead && x.PropertyType == typeof(bool) && x.DeclaringType == dataType);
                List<CachedGetter> cachedGetters = new List<CachedGetter>();
                foreach (var boolProp in boolProps)
                {
                    Func<T, bool> getter = (Func<T, bool>)Delegate.CreateDelegate(typeof(Func<T, bool>), null, boolProp.GetGetMethod());
                    Conditions.AddCondition(getter, boolProp.Name);
                    cachedGetters.Add(new CachedGetter { DisplayName = boolProp.Name, Getter = getter });
                }
                ReflectedConditionsCache.PropertyGetterCache.Add(dataType, cachedGetters);

            }
        }

        private void LoadFieldConditions(Type dataType)
        {
            if (ReflectedConditionsCache.FieldGetterCache.TryGetValue(dataType, out List<CachedGetter> getters))
            {               
                foreach (var getter in getters)
                {
                    Conditions.AddCondition(getter.Getter as Func<T, bool>, getter.DisplayName);
                }
            }
            else
            {

                var boolFields = dataType.GetFields().Where(x => x.FieldType == typeof(bool) && x.DeclaringType == dataType);
                List<CachedGetter> cache = new List<CachedGetter>();
                foreach (var field in boolFields)
                {
                    var getter = ReflectionHelper.GetGetFieldDelegate<T, bool>(field);
                    Conditions.AddCondition(getter, field.Name);
                    cache.Add(new CachedGetter { DisplayName = field.Name, Getter = getter });
                }
                ReflectedConditionsCache.FieldGetterCache.Add(dataType, cache);
            }
        }

        IDataTypeFunctionSubset IDataTypeFunctionSet.Actions => Actions;

        IDataTypeFunctionSubset IDataTypeFunctionSet.Conditions => Conditions;

        IDataTypeFunctionSubset IDataTypeFunctionSet.Evaluators => Evaluators;
        /// <summary>
        /// Add an action to a data type's action set. These actions can be used in the designer.
        /// </summary>
        /// <param name="action">Action the will be executed at runtime</param>
        /// <param name="actionName">Display name of action, must be unique to this set</param>
        public void AddAction(System.Action<T> action, string actionName) => Actions.AddAction(action, actionName);
        /// <summary>
        /// Add a logging action to a data type's action set. These actions can be used in the designer.
        /// </summary>
        /// <param name="messageExpression">Message function will be invoked and logged at runtime when action is executed.</param>
        /// <param name="actionName">Display name of action, must be unique to this set</param>
        public void AddLoggingAction(System.Func<T, string> messageExpression, string actionName) => Actions.AddLoggingMessage(messageExpression, actionName);
        /// <summary>
        /// Add a logging action to a data type's action set. These actions can be used in the designer.
        /// </summary>
        /// <param name="message">Message  be logged when action is executed.</param>
        /// <param name="actionName">Display name of action, must be unique to this set</param>
        public void AddLoggingAction(string message, string actionName) => Actions.AddLoggingMessage(message, actionName);
        /// <summary>
        /// Add a condition to a data type's condition set. These conditions can be used in the designer.
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="valueFunc"></param>
        /// <param name="name">Display name of condition, must be unique to this set</param>
        /// <returns>A condition builder which requires further configuration</returns>

        public IConditionBuilder<T, TProperty> AddCondition<TProperty>(Func<T, TProperty> valueFunc, string name) => Conditions.AddCondition(valueFunc, name);
        /// <summary>
        /// Add a condition to a data type's condition set. These conditions can be used in the designer.
        /// </summary>
        /// <param name="condition">condition function that is to be used at runtime</param>
        /// <param name="name">Display name of condition, must be unique to this set</param>
        public void AddCustomCondition(Func<T, bool> condition, string name) => Conditions.AddCustomCondition(name, condition);
        /// <summary>
        /// Add an evaluator to a data type's evaluator set. These evaulators can be used in the designer.
        /// </summary>
        /// <param name="evalFunc">Evaluation function which will be called at runtime.</param>
        /// <param name="evaluatorName">Display name of evaluator, must be unique to this set</param>
        public void AddEvaluator(Func<T, float> evalFunc, string evaluatorName) => Evaluators.AddEvaluator(evalFunc, evaluatorName);

        public List<FunctionModel> GetFunctionModels()
        {
            var functions = new List<FunctionModel>();
            Type dataType = typeof(T);

            var atr = dataType.GetCustomAttribute<EasyStateScriptAttribute>();
            if (atr is null)
            {
                Debug.LogWarning($"Data type {dataType.Name} does not have an easy state attribute and can not be processed.");
                return functions;
            }

            functions.AddRange(Actions.Actions.Select(x => new FunctionModel(x.AsFunction().Id)
            {

                Name = x.AsFunction().Name,
                DataTypeID = atr.ScriptID,
                FunctionType = FunctionType.Action,
                HasScriptFile = false,

            }));
            functions.AddRange(Conditions.Conditions.Select(x => new FunctionModel(x.AsFunction().Id)
            {

                Name = x.AsFunction().Name,
                DataTypeID = atr.ScriptID,
                FunctionType = FunctionType.Condition,
                HasScriptFile = false,

            }));
            functions.AddRange(Evaluators.Evaluators.Select(x => new FunctionModel(x.AsFunction().Id)
            {

                Name = x.AsFunction().Name,
                DataTypeID = atr.ScriptID,
                FunctionType = FunctionType.Evaluator,
                HasScriptFile = false,

            }));

            return functions;

        }


    }
}