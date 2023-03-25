using System;
using System.Collections.Generic;
using System.Linq;
using EasyState.Models;
using EasyState.DataModels;

namespace EasyState.Data.FileSync
{
    public class FunctionChanges : ChangeCollection<FunctionModel>
    {
        public FunctionChanges(List<DataTypeModel> dataTypes)
        {
            var allFunctions = LoadExistingFunctionsFromAssembly(dataTypes);
            foreach (var dataType in dataTypes)
            {
                var existingFuncOfThisType = allFunctions.Where(x => x.DataTypeID == dataType.Id).ToList();
                var cachedFuncsOfThisType = dataType.Functions.Where(x => x.DataTypeID == dataType.Id).ToList();
                foreach (var function in existingFuncOfThisType)
                {
                    var cachedFunc = cachedFuncsOfThisType.FirstOrDefault(x => x.Id == function.ScriptID);

                        //new script was added
                    if (cachedFunc == null)
                    {
                        HandleNewFunction(dataType, function);
                        continue;
                    }
                    //name changed
                    if (function.IsScript)
                    {
                        bool functionChanged = cachedFunc.Name != function?.Name ||
                                                cachedFunc.FullName != function.Type.FullName ||
                                                cachedFunc.AssemblyQualifiedName != function.Type.AssemblyQualifiedName ||
                                                CheckIfActionChanged(function, cachedFunc);


                        if (functionChanged)
                        {
                            cachedFunc.Name = function.Name;
                            cachedFunc.FullName = function.Type.FullName;
                            cachedFunc.AssemblyQualifiedName = function.Type.AssemblyQualifiedName;
                            if (IsParameterizedActionType(function))
                            {
                                TrySetParameterType(function, cachedFunc);                                
                            }
                            else
                            {
                                cachedFunc.ParameterType = null;
                                cachedFunc.EnumType = null;

                            }
                            ItemsUpdated.Add(cachedFunc);
                        }
                    }
                    //Nothing has changed
                }
                //check for functions deleted
                foreach (var function in cachedFuncsOfThisType)
                {
                    var scannedFunction = existingFuncOfThisType.FirstOrDefault(x => x.ScriptID == function.Id);
                    if (scannedFunction is null)
                    {
                        ItemsDeleted.Add(function);
                    }
                }
            }
        }

        private static List<System.Type> GetAllFunctions()
        {
            var functions = System.AppDomain.CurrentDomain.GetAllDerivedTypes(typeof(FunctionBase)).ToList();
            return functions;
        }

        private static FunctionType GetFunctionType(System.Type type)
        {
            var genericType = type.BaseType.GetGenericTypeDefinition();
            if (genericType == typeof(Models.Action<>) || genericType == typeof(Models.Action<,>))
            {
                return FunctionType.Action;
            }
            if (genericType == typeof(Condition<>))
            {
                return FunctionType.Condition;
            }
            if (genericType == typeof(Evaluator<>))
            {
                return FunctionType.Evaluator;
            }
            throw new System.NotImplementedException($"Was unable to convert type {type.Name} to a function type.");

        }

        private static List<ScriptModel> LoadExistingFunctionsFromAssembly(List<DataTypeModel> dataTypes)
        {
            var existingFunctions = GetAllFunctions().Where(x => !x.IsAbstract && x.Namespace != "EasyState.Core").ToList();
            var scriptModels = ScriptModel.ConvertTypeToScriptModels(existingFunctions);
            var nonScriptFunctions = dataTypes.SelectMany(dataTypeModel =>
            {
                var dataType = Type.GetType(dataTypeModel.AssemblyQualifiedName);
                if (dataType != null)
                {
                    //load additional non scripted conditions from data type                 

                    return dataTypeModel.LoadFunctionSet().Select(func => new ScriptModel(func));
                }
                else
                {
                    return new List<ScriptModel>();
                }
            });

            scriptModels.AddRange(nonScriptFunctions);
            return scriptModels;
        }

        private void HandleNewFunction(DataTypeModel dataType, ScriptModel function)
        {
            if (function.IsScript)
            {
                FunctionModel newFunction = new FunctionModel(function.ScriptID)
                {
                    Name = function.Type.Name,
                    DataTypeID = dataType.Id,
                    FunctionType = GetFunctionType(function.Type),
                    HasScriptFile = true,
                    AssemblyQualifiedName = function.Type.AssemblyQualifiedName,
                    FullName = function.Type.FullName,

                };
                if (IsParameterizedActionType(function))
                {
                    TrySetParameterType(function, newFunction);
                }
                ItemsAdded.Add(newFunction);

            }
            else
            {

                FunctionModel newFunction = new FunctionModel(function.ScriptID)
                {
                    Name = function.Name,
                    DataTypeID = dataType.Id,
                    FunctionType = function.FunctionType,
                };
                ItemsAdded.Add(newFunction);
            }
        }

        private static void TrySetParameterType(ScriptModel function, FunctionModel newFunction)
        {
            var parameterType = function.Type.BaseType.GenericTypeArguments[1];
            if (parameterType.IsEnum)
            {
                newFunction.EnumType = parameterType.AssemblyQualifiedName;
                newFunction.ParameterType = nameof(Enum);
            }
            else if (FunctionModel.ValidateParameterType(parameterType))
            {
                newFunction.ParameterType = parameterType.Name;
            }
            else
            {
                throw new InvalidOperationException($"\n--------\nAction named {function.Name} has an unsupported parameter type.\n---------");
            }
        }

        private static bool CheckIfActionChanged(ScriptModel function, FunctionModel cachedFunction)
        {
            if (function.FunctionType != FunctionType.Action)
            {
                return false;
            }
            //handle action with parameter
            if (IsParameterizedActionType(function))
            {
                //script has been changed so it now has a parameter.
                if (cachedFunction.HasParameter == false)
                {
                    return true;
                }
                var parameterType = function.Type.BaseType.GenericTypeArguments[1];


                //the parameter type was changed
                //if parameter is an enum it is stored slightly differently so we need to make sure that this is accounted for
                if (parameterType.Name != cachedFunction.ParameterType && (parameterType.IsEnum && cachedFunction.EnumType != parameterType.AssemblyQualifiedName))
                {
                    return true;
                }

            }
            else
            {
                //this action used to have a parameter but no longer does
                if (cachedFunction.HasParameter)
                {
                    return true;
                }

            }

            return false;

        }
        private static bool IsParameterizedActionType(ScriptModel script)
        {
            if (script.FunctionType != FunctionType.Action)
            {
                return false;
            }
            return script.Type.BaseType.GenericTypeArguments.Length == 2;
        }
    }
}