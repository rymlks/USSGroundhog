using EasyState.Models.DataType;

namespace EasyState.Models
{
    public interface IDataTypeEvaluator<T> :IEvaluator<T> where T : DataTypeBase
    {
        IDataTypeFunction<T> AsFunction();
    }
}