namespace EasyState.Models.DataType
{
    public interface IDataTypeFunction
    {
        string Id { get; }
        string Name { get; } 
    }
    public interface IDataTypeFunction<T> : IDataTypeFunction where T : DataTypeBase
    {
    }
}
