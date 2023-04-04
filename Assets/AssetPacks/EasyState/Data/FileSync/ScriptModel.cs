using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using EasyState.Models;
using EasyState.DataModels;

namespace EasyState.Data.FileSync
{
    public class ScriptModel
    {
        public string DataTypeID { get; set; }

        public FunctionType FunctionType { get; set; }

        public bool IsScript { get; set; }

        public string Name { get; set; }

        public string ScriptID { get; set; }

        public System.Type Type { get; set; }
         

        public ScriptModel(System.Type type, string scriptID)
        {
            Type = type;
            ScriptID = scriptID;
            if (type.IsSubclassOf(typeof(FunctionBase)))
            {
                try
                {
                    var dataType = type.BaseType.GenericTypeArguments[0];
                    EasyStateScriptAttribute dataTypeAtr = dataType.GetCustomAttribute<EasyStateScriptAttribute>();
                    DataTypeID = dataTypeAtr.ScriptID;


                }
                catch
                {
                    Debug.LogWarning("Could not construct script model for function of type " + type.Name);
                }
            }
            IsScript = true;
            Name =  Split(type.Name);
        }

        public ScriptModel(FunctionModel functionModel)
        {
            IsScript = false;
            Name = functionModel.Name;
            DataTypeID = functionModel.DataTypeID;
            ScriptID = functionModel.Id;
            FunctionType = functionModel.FunctionType;
        }

        public static List<ScriptModel> ConvertTypeToScriptModels(List<System.Type> typeList)
        {
            List<ScriptModel> models = new List<ScriptModel>();
            foreach (var script in typeList)
            {
                EasyStateScriptAttribute atr = script.GetCustomAttribute<EasyStateScriptAttribute>();
                if (atr == null)
                {
                    EasyStateIgnoreScriptAttribute ignoreAtr = script.GetCustomAttribute<EasyStateIgnoreScriptAttribute>();
                    if (ignoreAtr == null)
                    {
                        Debug.LogWarning($"'{script.Name}' is missing EasyStateScriptAttribute and cannot be  indexed.");
                    }
                    continue;
                }
                else
                {
                    var model = new ScriptModel(script, atr.ScriptID);
                    models.Add(model);
                }
            }
            return models;
        }

        static string Split(string input)
        {
            string result = "";

            for (int i = 0; i < input.Length; i++)
            {
                if (char.IsUpper(input[i]))
                {
                    result += ' ';
                }

                result += input[i];
            }

            return result.Trim();
        }
    }
}