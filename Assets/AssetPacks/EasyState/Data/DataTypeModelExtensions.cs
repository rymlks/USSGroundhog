using EasyState.Data.FileSync;
using System;
using System.Collections.Generic;
using System.Linq;
using EasyState.Models;
using EasyState.DataModels;
using EasyState.Models.DataType;

namespace EasyState.Data
{
    public static class DataTypeModelExtensions
    {
        /// <summary>
        /// Checks to see if any of the name details of the given <see cref="DataTypeModel"/> have been changed
        /// </summary>
        /// <param name="cachedModel"></param>
        /// <param name="scannedModel"></param>
        /// <returns></returns>
        public static bool HasBeenRenamed(this DataTypeModel cachedModel, ScriptModel scannedModel) =>

            cachedModel.Name != scannedModel.Type.Name || cachedModel.FullName != scannedModel.Type.FullName || cachedModel.AssemblyQualifiedName != scannedModel.Type.AssemblyQualifiedName;

        public static bool HasBeenRenamed(this DataTypeModel cachedModel, DataTypeModel newModel) =>

          cachedModel.Name != newModel.Name || cachedModel.FullName != newModel.FullName || cachedModel.AssemblyQualifiedName != newModel.AssemblyQualifiedName;

        public static List<string> GetFloatFields(this DataTypeModel model)
        {
            var dataType = Type.GetType(model.AssemblyQualifiedName);
            var fields = dataType.GetFields().Where(x=> x.FieldType == typeof(float)).Select(x=> x.Name).ToList();
            return fields;
        }
        public static List<FunctionModel> LoadFunctionSet(this DataTypeModel model)
        {
            var functions = new List<FunctionModel>();
            var dataType = Type.GetType(model.AssemblyQualifiedName); 
            var dataTypeInstance = TypePoolCache.DefaultCache.GetObjectInstance(CachedInstanceType.DataType, dataType);

            var setType = typeof(DataTypeFunctionSet<>).MakeGenericType(dataType);
            var setFields = dataType.GetFields().Where(x => x.FieldType == setType);

            foreach (var setFieldInfo in setFields)
            {
                var set = (IFunctionModelSet)setFieldInfo.GetValue(dataTypeInstance);
                functions.AddRange(set.GetFunctionModels());
            }
            return functions;
        }          
        public static List<IDataTypeFunctionSet> GetAllDataTypeFunctionSets<TDataType>(this DataTypeModel model, TDataType dataTypeInstance) where TDataType : DataTypeBase
        {
            var sets = new List<IDataTypeFunctionSet>();
            var dataType = Type.GetType(model.AssemblyQualifiedName);             
            var setType = typeof(IDataTypeFunctionSet);
            var setFields = dataType.GetFields().Where(x => x.FieldType.GetInterface(nameof(IDataTypeFunctionSet)) != null);
            foreach (var setFieldInfo in setFields)
            {
                var set = (IDataTypeFunctionSet)setFieldInfo.GetValue(dataTypeInstance);
                sets.Add(set);
            }
            return sets;
        }
    }
}