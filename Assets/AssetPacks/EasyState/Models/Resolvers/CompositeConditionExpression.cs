using EasyState.DataModels;
using System;
using System.Collections.Generic;

namespace EasyState.Models.Resolvers
{
    public class CompositeConditionExpression<T> : IConditionalExpression<T> where T : DataTypeBase
    {

        public ConditionalLogicType LogicType { get; }

        private readonly IConditionalExpression<T> _initialExpression;
        private readonly List<IConditionalExpression<T>> _additionalExpressions = new List<IConditionalExpression<T>>();
        private readonly int _additionalExpressionCount;
        public CompositeConditionExpression(IConditionalExpression<T> initialExpression, List<IConditionalExpression<T>> additionalExpressions = null, ConditionalLogicType logicType = ConditionalLogicType.AND)
        {
            _initialExpression = initialExpression;
            if (_additionalExpressions != null)
            {
                _additionalExpressions = additionalExpressions;
            }
            LogicType = logicType;
            if (initialExpression == null)
            {
                throw new ArgumentNullException(nameof(initialExpression));
            }
            _additionalExpressionCount = additionalExpressions.Count;
        }


        public bool Evaluate(T data)
        {
            bool result = _initialExpression.Evaluate(data);
            if (_additionalExpressions.Count > 0)
            {
                for (int i = 0; i < _additionalExpressionCount; i++)
                {
                    var exp = _additionalExpressions[i];
                    if (exp.LogicType == ConditionalLogicType.AND)
                    {
                        result &= exp.Evaluate(data);
                    }
                    else
                    {
                        result |= exp.Evaluate(data);
                    }
                }
            }
            return result;
        }

        public bool BaseEvaluate(DataTypeBase data) => Evaluate((T)data);
    }
}
