namespace EasyState.Models
{
    public interface IAction<T> :IAction where T:DataTypeBase
    {
        void Act(T data);
    }
    public interface IAction
    {
        void BaseAct(DataTypeBase data);
        string GetName();
    }
}