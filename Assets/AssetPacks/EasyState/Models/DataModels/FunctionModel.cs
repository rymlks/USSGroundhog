using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace EasyState.DataModels
{
    public class FunctionModel
    {
        [JsonProperty("a")]
        public string Id { get; }
        [JsonProperty("b")]
        public string Name { get; set; }
        [JsonProperty("c")]
        public string AssemblyQualifiedName { get; set; }
        [JsonProperty("d")]
        public string DataTypeID { get; set; }
        [JsonProperty("e")]
        public string FullName { get; set; }
        [JsonProperty("f")]
        public FunctionType FunctionType { get; set; }
        [JsonIgnore]
        public bool HasParameter => !string.IsNullOrEmpty(ParameterType);
        [JsonProperty("g")]
        public string ParameterType { get; set; }
        [JsonProperty("h")]
        public bool HasScriptFile { get; set; }
        [JsonProperty("i")]
        public string EnumType { get; set; }
        private static System.Type[] _supportedTypes = new System.Type[]
        {
            typeof(int),
            typeof(float),
            typeof(string),
            typeof(bool),
            typeof(System.Enum),
        };
        public static bool ValidateParameterType(System.Type paramterType)
        {

            if (!_supportedTypes.Contains(paramterType))
            {
                return false;
            }
            return true;
        }

        public static readonly FunctionModel AlwaysTrue = new FunctionModel(Models.AlwaysTrue.Id)
        {
            Name = nameof(Models.AlwaysTrue),
            FunctionType = FunctionType.Condition,
            HasScriptFile = true,
            AssemblyQualifiedName = typeof(Models.AlwaysTrue).AssemblyQualifiedName,
            FullName = typeof(Models.AlwaysTrue).FullName,
            DataTypeID = Models.DataTypeBase.DATA_TYPE_BASE_ID,

        };

        public static readonly FunctionModel NullFunction = new FunctionModel("NULL")
        {
            Name = "Select...",
            FunctionType = FunctionType.Action,
            DataTypeID = "-1",
            HasScriptFile = false
        };

        public FunctionModel(string id)
        {
            Id = id;
        }

        public override bool Equals(object obj)
        {
            return obj is FunctionModel model &&
                   Id == model.Id &&
                   Name == model.Name &&
                   DataTypeID == model.DataTypeID &&
                   HasScriptFile == model.HasScriptFile &&
                   FunctionType == model.FunctionType &&
                   FullName == model.FullName &&
                   AssemblyQualifiedName == model.AssemblyQualifiedName &&
                   EnumType == model.EnumType &&
                   ParameterType == model.ParameterType;
        }

        public override int GetHashCode()
        {
            int hashCode = 850568138;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Id);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(DataTypeID);
            hashCode = hashCode * -1521134295 + HasScriptFile.GetHashCode();
            hashCode = hashCode * -1521134295 + FunctionType.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FullName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(AssemblyQualifiedName);
            return hashCode;
        }
        public object ParseValue(string valueAsString)
        {
            if (string.IsNullOrEmpty(valueAsString))
            {
                return null;
            }
            switch (ParameterType)
            {
                case nameof(System.String):
                    return valueAsString;
                case nameof(System.Int32):
                    return int.Parse(valueAsString);
                case nameof(System.Single):
                    return float.Parse(valueAsString);
                case nameof(System.Boolean):
                    return bool.Parse(valueAsString);
                case nameof(System.Enum):
                    return System.Enum.Parse(System.Type.GetType(EnumType), valueAsString);
                default:
                    throw new System.NotSupportedException("This parameter type is not supported");
            }

        }
    }
}