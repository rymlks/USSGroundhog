using Newtonsoft.Json;

namespace EasyState.DataModels
{
    public class NoteData : MoveableData
    {
        [JsonProperty("a")]
        public string Contents { get; set; }

        [JsonProperty("b")]
        public ColorData NoteColor { get; set; }

        [JsonProperty("c")]
        public ColorData TextColor { get; set; }

    }
}