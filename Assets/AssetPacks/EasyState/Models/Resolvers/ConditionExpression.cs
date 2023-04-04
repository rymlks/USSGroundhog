using EasyState.DataModels;

namespace EasyState.Models.Resolvers
{
    public class ConditionExpression<T> : IConditionalExpression<T> where T : DataTypeBase
    {
        public ConditionalLogicType LogicType { get; }
        private readonly bool _expectedResult;
        private readonly ICondition _condition;

        public ConditionExpression(bool expectedResult, ICondition condition, ConditionalLogicType logicType = ConditionalLogicType.AND)
        {
            _expectedResult = expectedResult;
            _condition = condition;
            LogicType = logicType;
        }

        public bool Evaluate(T data) => _expectedResult == _condition.BaseEvaluate(data);

        public bool BaseEvaluate(DataTypeBase data) => Evaluate((T)data);
    }
}
