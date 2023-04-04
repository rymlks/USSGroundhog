using EasyState.DataModels;
namespace EasyState.Core.Models
{
    public interface IDataModel<TDataModel> where TDataModel: ModelData 
    {
        TDataModel Serialize();
    } 
}