using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyState.DataModels
{
    public static class DataTypeModelExtensions
    {
        public static List<FunctionModel> GetActions(this DataTypeModel model) => model.Functions.Where(x => x.FunctionType == FunctionType.Action).ToList();

        public static List<FunctionModel> GetConditions(this DataTypeModel model) => model.Functions.Where(x => x.FunctionType == FunctionType.Condition).ToList();

        public static List<FunctionModel> GetEvaluators(this DataTypeModel model) => model.Functions.Where(x => x.FunctionType == FunctionType.Evaluator).ToList();
    }

    public class DataTypeModel : ModelData
    {
        [JsonProperty("a")]
        public string AssemblyQualifiedName { get; set; }
        /// <summary>
        /// class name with namespace
        /// </summary>
        [JsonProperty("b")]
        public string FullName { get; set; }
        [JsonProperty("c")]
        public List<FunctionModel> Functions { get; set; } = new List<FunctionModel>();
        [JsonProperty("d")]
        public string ParentDataTypeID { get; set; }

        public DataTypeModel(string id = null)
        {
            if (id == null)
            {
                Id = Guid.NewGuid().ToString();
            }
            else
            {
                Id = id;
            }
        }
    }
}