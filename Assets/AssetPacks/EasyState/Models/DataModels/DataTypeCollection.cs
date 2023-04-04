using EasyState.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EasyState.DataModels
{
    public class DataTypeCollection
    {
        public List<DataTypeModel> DataTypes = new List<DataTypeModel>();

        public DataTypeCollection()
        {
        }

        public DataTypeCollection(List<DataTypeModel> dataTypes)
        {
            DataTypes = dataTypes;
            //LoadInheritedFunctions();
        }

        public void LoadInheritedFunctions()
        {
            foreach (var dataType in DataTypes)
            {
                CheckForParentDataType(dataType);
                dataType.Functions = dataType.Functions.Distinct().ToList();
                if(dataType.Id != DataTypeBase.DATA_TYPE_BASE_ID)
                {
                    var baseType = DataTypes.First(x=> x.Id == DataTypeBase.DATA_TYPE_BASE_ID);
                   dataType.Functions.AddRange(baseType.Functions);
                }
            }
        }

        /// <summary>
        /// Adds all of the parent's functions to the child's function list
        /// </summary>
        /// <param name="dataType"></param>
        /// <param name="recursionCount"></param>
        private void CheckForParentDataType(DataTypeModel dataType, int recursionCount = 10)
        {
            if (recursionCount == 0)
            {
                Debug.LogWarning("Easy State does not support inheritance above 10, if required you can up the parameter 'recursionCount' on this model ");
                return;
            }
            if (dataType.ParentDataTypeID != null)
            {
                var parentDataType = DataTypes.FirstOrDefault(x => x.Id == dataType.ParentDataTypeID);
                if (parentDataType is null)
                {
                    Debug.LogError($"Couldn't find parent data type for {dataType.Name} with ID of {dataType.ParentDataTypeID}");

                    return;
                }
                if (parentDataType.ParentDataTypeID != null)
                {
                    CheckForParentDataType(parentDataType, recursionCount--);
                }
                dataType.Functions.AddRange(parentDataType.Functions);
            }
        }
    }
}