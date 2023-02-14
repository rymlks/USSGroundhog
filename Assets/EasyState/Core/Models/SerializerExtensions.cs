using EasyState.DataModels;

namespace EasyState.Core.Models
{
    public static class SerializerExtensions
    {
        public static void SetModelData(this ModelData dataObject, Model model)
        {
            dataObject.Id = model.Id;
            dataObject.Name = model.Name;
        }
        public static void SetModelData(this SelectableData dataObject, SelectableModel model)
        {
            dataObject.SetModelData((Model)model);
            dataObject.Selected = model.Selected;
        }
        public static void SetModelData(this MoveableData dataObject, MoveableModel model)
        {
            dataObject.SetModelData((SelectableModel)model);
            dataObject.Position = model.Position;
        }
    }
}
