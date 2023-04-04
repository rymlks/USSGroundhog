namespace EasyState.Models
{

    [EasyStateScript(Id)]
    public class AlwaysTrue : Condition<DataTypeBase> 
    {
        public const string Id = "c85a2eb2-766e-49cb-8159-969bc0b2e851";
        public override bool Evaluate(DataTypeBase data) => true;
    }
    [EasyStateIgnoreScript]
    public class AlwaysTrueCondition<T> : AlwaysTrue, ICondition<T> where T : DataTypeBase
    {
        public bool Evaluate(T data)=> true;
    }
}