using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EasyState.Models;
using EasyState.DataModels;
using System;

namespace EasyState.Data.FileSync
{
    public class DataTypeChanges : ChangeCollection<DataTypeModel>
    {
        public event System.Action<string> LogWarning;
        private static Type _dataTypeBase = typeof(DataTypeBase);
        public DataTypeChanges(List<DataTypeModel> dataTypes)
        {
            var scannedDataTypes = LoadExistingDataTypesFromAssembly();

            foreach (var dataType in scannedDataTypes)
            {
                var cachedType = dataTypes.FirstOrDefault(x => x.Id == dataType.ScriptID);
                //new script was added
                if (cachedType == null)
                {
                    AddDataType(dataType);
                    continue;
                }
                //data type has been renamed

                if (cachedType.HasBeenRenamed(dataType))
                {
                    RenameDataType(dataType, cachedType);
                }
                if (dataType.Type.BaseType != _dataTypeBase && dataType.Type != _dataTypeBase)
                {
                    CheckParentDataType(dataType, cachedType);
                }
                //No changes detected
            }
            // check for data types deleted

            HandleDeletedDataTypes(scannedDataTypes, dataTypes);
        }

        private static List<Type> GetAllDataTypes()
        {
            var dataTypes = AppDomain.CurrentDomain.GetAllDerivedTypes(_dataTypeBase).ToList();
            dataTypes.Add(typeof(DataTypeBase));
            return dataTypes;
        }

        private static List<ScriptModel> LoadExistingDataTypesFromAssembly()
        {
            var existingDataTypes = GetAllDataTypes();
            return ScriptModel.ConvertTypeToScriptModels(existingDataTypes);
        }

        private void AddDataType(ScriptModel dataType)
        {
            var parentDataType = dataType.Type.BaseType;
            string parentDataTypeID = null;
            if (parentDataType != _dataTypeBase)
            {
                var parentAtr = parentDataType.GetCustomAttribute<EasyStateScriptAttribute>();
                if (parentAtr != null)
                {
                    parentDataTypeID = parentAtr.ScriptID;
                }
                else
                {
                    LogWarning?.Invoke($"{dataType.Type.Name} inherits from {parentDataType.Name}, who is missing its EasyStateScriptAttribute the parent ID cannot be indexed.");
                }
            }
            DataTypeModel newDataType = new DataTypeModel(dataType.ScriptID)
            {
                Name = dataType.Type.Name,
                FullName = dataType.Type.FullName,
                AssemblyQualifiedName = dataType.Type.AssemblyQualifiedName,
                ParentDataTypeID = parentDataTypeID
            };
            ItemsAdded.Add(newDataType);
        }

        private void CheckParentDataType(ScriptModel dataType, DataTypeModel cachedType)
        {
            var parentDataType = dataType.Type.BaseType;
            var parentAtr = parentDataType.GetCustomAttribute<EasyStateScriptAttribute>();
            if (parentAtr != null)
            {
                if (cachedType.ParentDataTypeID != parentAtr.ScriptID)
                {
                    cachedType.ParentDataTypeID = parentAtr.ScriptID;
                    ItemsUpdated.Add(cachedType);
                }
            }
            else
            {
                LogWarning?.Invoke($"{dataType.Type.Name} inherits from {parentDataType.Name}, who is missing its EasyStateScriptAttribute the parent ID cannot be indexed.");
            }
        }

        private void HandleDeletedDataTypes(List<ScriptModel> scannedDataTypes, List<DataTypeModel> cachedDataTypes)
        {
            foreach (var dataType in cachedDataTypes)
            {
                var scannedDataType = scannedDataTypes.FirstOrDefault(x => x.ScriptID == dataType.Id);
                //data type deleted
                if (scannedDataType is null)
                {
                    ItemsDeleted.Add(dataType);
                }
            }
        }

        private void RenameDataType(ScriptModel dataType, DataTypeModel cachedType)
        {
            cachedType.FullName = dataType.Type.FullName;
            cachedType.Name = dataType.Type.Name;
            cachedType.AssemblyQualifiedName = dataType.Type.AssemblyQualifiedName;
            ItemsUpdated.Add(cachedType);
        }
    }
}