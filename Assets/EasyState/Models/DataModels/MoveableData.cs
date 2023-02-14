using Newtonsoft.Json;

namespace EasyState.DataModels
{
    public abstract class MoveableData : SelectableData
    {
        [JsonProperty("u")]
        public Vector2Data Position { get; set; }
  
    }
}