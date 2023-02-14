using EasyState.Models.DataType;

namespace EasyState.Models
{
    public interface IDataTypeCondition<T> :ICondition<T> where T : DataTypeBase
    {
        IDataTypeFunction<T> AsFunction();
    }
}