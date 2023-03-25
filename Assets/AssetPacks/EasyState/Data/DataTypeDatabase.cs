using EasyState.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace EasyState.Data
{
    public class DataTypeDatabase : DatabaseFile<DataTypeCollection>
    {
        public DataTypeDatabase() : base("dataTypes.json")
        {
        }

        protected override void InitializeDatabaseFile(DataTypeCollection dataModel)
        {
            dataModel.LoadInheritedFunctions();
        }
        protected override DataTypeCollection SanitizeData(DataTypeCollection data)
        {
            if (data.DataTypes != null)
            {
                foreach (var dataType in data.DataTypes)
                {
                    //remove any inherited functions from being saved.
                    var functionsToDelete = dataType.Functions.Where(x => x.DataTypeID != dataType.Id).ToList();
                    foreach (var func in functionsToDelete)
                    {
                        dataType.Functions.Remove(func);
                    }
                }
            }
            return data;
        }
        public void DeleteDataType(DataTypeCollection data, DataTypeModel dataTypeToDelete)
        {
            var childDataTypes = GetChildDataTypes(data, dataTypeToDelete);

            if (childDataTypes.Any())
            {
                foreach (var child in childDataTypes)
                {
                    if(child.ParentDataTypeID == dataTypeToDelete.Id)
                    {
                        child.ParentDataTypeID = null;
                    }
                }
            }
            var allDesigns = new DesignCollectionDatabase().Load();
            var designsToDelete = allDesigns.Designs.Where(x => x.DataTypeID == dataTypeToDelete.Id).ToList();

            foreach (var design in designsToDelete)
            {
                var designDb = new DesignDatabase(design.Id);
                designDb.DeleteFile();
                LogFileChange("A data type was deleted and an associated design was found and also deleted.");
            }
            //validate dependent behaviors on file change
            var behaviorDb = new BehaviorCollectionDatabase();
            var behaviorCollection = behaviorDb.Load();
            if (behaviorCollection.Behaviors.Any())
            {
                bool changeDetected = false;
                var behaviorsToDelete = behaviorCollection.Behaviors.Where(x =>
                {
                    //one of the data types that inherited from the deleted data type has a behavior
                    return childDataTypes.Any(y => y.Id == x.DataTypeId);
                   
                }).ToList();

                foreach (var behavior in behaviorsToDelete)
                {
                    LogFileChange($" Behavior '{behavior.BehaviorName}' has a parent data type that was deleted and is therefore being deleted.");
                    changeDetected = true;
                    behaviorCollection.Behaviors.Remove(behavior);
                }

                if (changeDetected)
                {
                    behaviorDb.Save(behaviorCollection);
                }

            }
            data.DataTypes.Remove(dataTypeToDelete);
            Save(data);

        }
        private List<DataTypeModel> GetChildDataTypes(DataTypeCollection data, DataTypeModel dataType)
        {
            List<DataTypeModel> dataTypes = new List<DataTypeModel>();
            dataTypes.AddRange(data.DataTypes.Where(x => x.ParentDataTypeID == dataType.Id));
            foreach (var child in new List<DataTypeModel>(dataTypes))
            {
                dataTypes.AddRange(GetChildDataTypes(data, child));
            }
            return dataTypes;
        }
     
    }
}