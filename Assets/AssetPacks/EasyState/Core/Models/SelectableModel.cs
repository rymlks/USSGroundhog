using EasyState.DataModels;

namespace EasyState.Core.Models
{
    public abstract class SelectableModel : Model
    {
        public bool Selected { get; set; }

        public SelectableModel(SelectableData data) : base(data)
        {
            Selected = data.Selected;
        }
        public SelectableModel(string id) : base(id)
        {
        }
       
    }
}