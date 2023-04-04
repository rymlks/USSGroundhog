using EasyState.Models.DataType;

namespace EasyState.Models
{
    public interface IDataTypeAction<T>: IAction<T> where T : DataTypeBase
    {
        IDataTypeFunction<T> AsFunction();
    }
}