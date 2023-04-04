
using Newtonsoft.Json;
using System.Collections.Generic;

namespace EasyState.DataModels
{
    public class GroupData : MoveableData
    {
        [JsonProperty("a")]
        public List<string> ChildrenIDs { get; set; } = new List<string>();
    }
}