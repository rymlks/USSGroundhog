using System.Collections.Generic;

namespace EasyState.Models.DataType
{
    public interface IDataTypeFunctionSubset
    {
        IReadOnlyList<IDataTypeFunction> Set { get; }
    }
    public interface IDataTypeFunctionSubset<T>: IDataTypeFunctionSubset where T:DataTypeBase
    {
        IReadOnlyList<IDataTypeFunction<T>> GenericSet { get; }
    }
    public interface IDataTypeFunctionSet
    {
        IDataTypeFunctionSubset Actions { get; }
        IDataTypeFunctionSubset Conditions { get; }
        IDataTypeFunctionSubset Evaluators { get; }

    }
}
