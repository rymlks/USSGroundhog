#if UNITY_EDITOR
using EasyState.DataModels;
using EasyState.Settings;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace EasyState.Data.FileSync
{
    public static class ChangeHandler
    {
        private static EasyStateSettings _settings = EasyStateSettings.Instance;
        private static bool _logMessages = _settings.LogFileChanges;

        public static void HandleChanges(DataTypeChanges changes, DataTypeCollection dataTypes, List<DesignData> designs, DesignerWindowState windowState)
        {
            var db = new DataTypeDatabase();
            foreach (var item in changes.ItemsDeleted)
            {
                if (UnityEditor.EditorUtility.DisplayDialog("Easy State - Missing Data Type", $"Easy State could not find Data Type named {item.Name}," +
                    $" was it deleted? Clicking yes will delete data type and dependent components(i.e Designs, Behaviors, etc). Clicking no will ignore missing file until next scan.", "Yes", "No"))
                {
                    var dataTypeToDelete = dataTypes.DataTypes.First(x => x.Id == item.Id);
                    db.DeleteDataType(dataTypes, item);
                }
            }
            foreach (var item in changes.ItemsUpdated)
            {
                var dataTypeToUpdate = dataTypes.DataTypes.First(x => x.Id == item.Id);
                if (dataTypeToUpdate.HasBeenRenamed(item))
                {
                    LogMessage($"DataType renamed to : {dataTypeToUpdate.FullName}");
                    dataTypeToUpdate.Name = item.Name;
                    dataTypeToUpdate.FullName = item.FullName;
                    dataTypeToUpdate.AssemblyQualifiedName = item.AssemblyQualifiedName;
                }
                if (dataTypeToUpdate.ParentDataTypeID != item.ParentDataTypeID)
                {
                    dataTypeToUpdate.ParentDataTypeID = item.ParentDataTypeID;
                    LogMessage($"DataType parent updated to : {dataTypeToUpdate.ParentDataTypeID}");
                }
            }
            foreach (var item in changes.ItemsAdded)
            {
                dataTypes.DataTypes.Add(item);
                LogMessage($"New DataType named : {item.FullName} detected and indexed");
            }
            db.Save(dataTypes);
        }

        public static void HandleChanges(FunctionChanges changes, DataTypeCollection dataTypes, List<DesignData> designs, DesignerWindowState windowState)
        {
            foreach (var item in changes.ItemsDeleted)
            {
                HandleDeletedFunctions(dataTypes, designs, windowState, item);
            }
            foreach (var item in changes.ItemsUpdated)
            {
                var dataType = dataTypes.DataTypes.First(x => x.Id == item.DataTypeID);
                var funcToUpdate = dataType.Functions.First(x => x.Id == item.Id);
                LogMessage($"Function named : {item.FullName} has been renamed");
                funcToUpdate.Name = item.Name;
            }
            foreach (var item in changes.ItemsAdded)
            {
                var dataType = dataTypes.DataTypes.First(x => x.Id == item.DataTypeID);
                dataType.Functions.Add(item);
                LogMessage($"Function named : {item.Name} has been detected and added to database");
            }
            var db = new DataTypeDatabase();
            db.Save(dataTypes);
        }
        private static void HandleDeletedFunctions(DataTypeCollection dataTypes, List<DesignData> designs, DesignerWindowState windowState, FunctionModel item)
        {

            foreach (var design in designs)
            {
                ValidateDesign(item, design, false);
                var designDB = new DesignDatabase(design.Id);
                designDB.Save(design);
            }

            foreach (var design in windowState.OpenDesigns)
            {
                ValidateDesign(item, design, true);
            }
            var db = new DesignWindowDatabase();
            db.Save(windowState);

            var dataType = dataTypes.DataTypes.First(x => x.Id == item.DataTypeID);
            var funcToDelete = dataType.Functions.First(x => x.Id == item.Id);
            dataType.Functions.Remove(funcToDelete);
            LogMessage($"Function named : {item.Name} was deleted from database");
        }

        private static void LogMessage(string message)
        {
            if (_logMessages)
            {
                Debug.Log(message);
            }
        }

        private static void ValidateDesign(FunctionModel item, DesignData design, bool isOpen)
        {
            foreach (var node in design.Nodes)
            {
                foreach (var action in node.NodeActions)
                {
                    if (action.ConditionID == item.Id)
                    {
                        action.ConditionID = null;
                        LogMessage($"{(isOpen ? "Open" : string.Empty)} Design : {design.Name} - Node named : {node.Name} had a conditional action that used the condition :{item.Name}. This value is cleared because function was deleted");
                    }
                    if (action.ActionID == item.Id)
                    {
                        action.ActionID = null;
                        LogMessage($"{(isOpen ? "Open" : string.Empty)} Design : {design.Name} - Node named : {node.Name} had an action :{item.Name}. This value is cleared because the action was deleted");
                    }
                }
                foreach (var connection in node.Connections)
                {
                    var set = connection.ConditionalExpression;
                    if (set == null)
                    {
                        continue;
                    }
                    if (set.InitialConditionalRow != null)
                    {
                        var initRow = set.InitialConditionalRow;
                        ValidateExpressionRow(item, design, node, initRow, isOpen);
                        if (set.AdditionalRows != null)
                        {
                            foreach (var row in set.AdditionalRows)
                            {
                                ValidateExpressionRow(item, design, node, row, isOpen);
                            }
                        }
                    }
                }
            }
        }

        private static void ValidateExpressionRow(FunctionModel item, DesignData design, NodeData node, ConditionalExpressionRowData row, bool isOpen)
        {
            if (row.InitialExpressionData != null)
            {
                if (row.InitialExpressionData.ConditionID == item.Id)
                {
                    row.InitialExpressionData.ConditionID = null;
                    LogMessage($"{(isOpen ? "Open " : string.Empty)}Design : {design.Name} - Node named : {node.Name} used condition :{item.Name}. This value is cleared because the condition was deleted");
                }
                if (row.AdditionalExpressions != null)
                {
                    foreach (var rowExp in row.AdditionalExpressions)
                    {
                        if (rowExp.ConditionID == item.Id)
                        {
                            rowExp.ConditionID = null;
                            LogMessage($"{(isOpen ? "Open " : string.Empty)}Design : {design.Name} - Node named : {node.Name} used condition :{item.Name}. This value is cleared because the condition was deleted");
                        }
                    }
                }
            }
        }
    }
} 
#endif