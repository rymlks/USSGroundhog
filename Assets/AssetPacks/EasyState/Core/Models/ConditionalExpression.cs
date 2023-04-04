using EasyState.DataModels;
using System;

namespace EasyState.Core.Models
{

    public class ConditionalExpression : Model, IDataModel<ConditionalExpressionData>
    {
        public FunctionModel Condition { get; set; }
        public ConditionalLogicType ConditionalLogicType { get; set; }
        public bool ExpectedResult { get; set; } = true;

        public ConditionalExpression(ConditionalExpressionData data) : base(data)
        {
            ExpectedResult = data.ExpectedResult;
            ConditionalLogicType = data.ConditionalLogicType;
        }

        public ConditionalExpression(string id = null) : base(id)
        {
        }
        /// <summary>
        /// Create deep copy from source expression
        /// </summary>
        /// <param name="sourceExpression"></param>
        public ConditionalExpression(ConditionalExpression sourceExpression) : base(Guid.NewGuid().ToString())
        {
            Condition = sourceExpression.Condition;
            ConditionalLogicType = sourceExpression.ConditionalLogicType;
            ExpectedResult = sourceExpression.ExpectedResult;
        }

        public ConditionalExpressionData Serialize()
        {
            var data = new ConditionalExpressionData()
            {
                ConditionalLogicType = ConditionalLogicType,
                ConditionID = Condition?.Id,
                ExpectedResult = ExpectedResult
            };
            data.SetModelData(this);
            return data;
        }
    }
}