
namespace EasyState.Models
{
    [System.Serializable]
    public class Behavior<T> where T : DataTypeBase
    {
        public int BehaviorID { get; set; }
        public State<T> InitialState;
        public State<T>[] States;
    }
}