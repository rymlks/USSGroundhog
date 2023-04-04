using EasyState.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace EasyState.Data.FileSync
{
    public class ChangeCollection<T>
    {
        public bool ChangesDetected => ItemsDeleted.Any() || ItemsUpdated.Any() || ItemsAdded.Any();
        public List<T> ItemsAdded { get; set; } = new List<T>();
        public List<T> ItemsDeleted { get; set; } = new List<T>();
        public List<T> ItemsUpdated { get; set; } = new List<T>();

        public override string ToString()
        {
            return $"Changes Detected : {ChangesDetected}, Items Deleted : {ItemsDeleted.Count}, Items Updated : {ItemsUpdated.Count}, Items Added : {ItemsAdded.Count}";
        }
    }

    public class ChangeModel
    {
        public bool ChangesDetected => FunctionChanges.ChangesDetected || DataTypeChanges.ChangesDetected;
        public ChangeCollection<DataTypeModel> DataTypeChanges = new ChangeCollection<DataTypeModel>();
        public ChangeCollection<FunctionModel> FunctionChanges = new ChangeCollection<FunctionModel>();
    }
}