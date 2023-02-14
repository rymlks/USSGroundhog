namespace EasyState.Core.Models
{
    public interface IDuplicable
    {
        IDuplicable Duplicate(Design design);
        bool CanDuplicate { get; }
    }
   
}
