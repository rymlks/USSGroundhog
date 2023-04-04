using EasyState.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyState.Core.Models
{
    public class ConditionalExpressionRow : Model, IDataModel<ConditionalExpressionRowData>
    {
        public List<ConditionalExpression> AdditionalExpressions { get; set; } = new List<ConditionalExpression>();
        public ConditionalLogicType ConditionalLogicType { get; set; }
        public bool HasAdditionalExpressions => AdditionalExpressions.Count > 0;
        public ConditionalExpression InitialExpression { get; set; }

        public ConditionalExpressionRow(ConditionalExpressionRowData data) : base(data)
        {
            ConditionalLogicType = data.ConditionalLogicType;
        }

        public ConditionalExpressionRow(string id = null) : base(id)
        {
            InitialExpression = new ConditionalExpression();
        }
        public ConditionalExpressionRow(ConditionalExpressionRow sourceRow) : base(Guid.NewGuid().ToString())
        {
            AdditionalExpressions = sourceRow.AdditionalExpressions.Select(x => new ConditionalExpression(x)).ToList();
            ConditionalLogicType = sourceRow.ConditionalLogicType;
            InitialExpression = new ConditionalExpression(sourceRow.InitialExpression);
        }

        public ConditionalExpressionRowData Serialize()
        {
            var data = new ConditionalExpressionRowData
            {

                ConditionalLogicType = this.ConditionalLogicType,
                InitialExpressionData = InitialExpression?.Serialize(),
                AdditionalExpressions = AdditionalExpressions.Select(x => x.Serialize()).ToList()

            };
            data.SetModelData(this);

            return data;
        }
    }
}