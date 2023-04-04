using Newtonsoft.Json;

namespace EasyState.DataModels
{
    public abstract class SelectableData : ModelData
    {
        [JsonProperty("v")]
        public bool Selected { get; set; }
       
    }
}