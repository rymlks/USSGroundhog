namespace EasyState.Models
{
    public interface IResolver<T> where T: DataTypeBase
    {
        State<T> Resolve(T data);
    }
}