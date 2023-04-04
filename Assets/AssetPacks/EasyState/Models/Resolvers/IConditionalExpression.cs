using EasyState.DataModels;

namespace EasyState.Models.Resolvers
{
    public interface IConditionalExpression<T> : ICondition<T> where T : DataTypeBase
    {
        ConditionalLogicType LogicType { get; }
    }
}
