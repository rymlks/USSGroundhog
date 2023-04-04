using Newtonsoft.Json;

namespace EasyState.DataModels
{
    public abstract class ModelData
    {
        [JsonProperty("y")]
        public string Id { get; set; }

        [JsonProperty("x")]
        public string Name { get; set; }
     
    }
}