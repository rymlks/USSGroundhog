using Newtonsoft.Json;
using System.Collections.Generic;

namespace EasyState.DataModels
{
    public class DesignerWindowState
    {
        [JsonProperty("c")]
        public bool LoaderOpen { get; set; }

        [JsonProperty("a")]
        public List<DesignData> OpenDesigns { get; set; } = new List<DesignData>();

        [JsonProperty("b")]
        public int SelectedTab { get; set; }
    }
}