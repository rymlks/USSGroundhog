using EasyState.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyState.Core.Models
{
    public class ConditionalExpressionSet : Model, IDataModel<ConditionalExpressionSetData>
    {
        public List<ConditionalExpressionRow> AdditionalRows { get; set; } = new List<ConditionalExpressionRow>();
        public bool HasAdditionalRows => AdditionalRows.Count > 0;
        public ConditionalExpressionRow InitialConditionalRow { get; set; }

        public ConditionalExpressionSet(ConditionalExpressionSetData data) : base(data)
        {
        }

        public ConditionalExpressionSet(string id = null) : base(id)
        {
            InitialConditionalRow = new ConditionalExpressionRow();
        }
        /// <summary>
        /// Create a deep copy from the source set
        /// </summary>
        /// <param name="sourceSet"></param>
        public ConditionalExpressionSet(ConditionalExpressionSet sourceSet) : base(Guid.NewGuid().ToString())
        {
            AdditionalRows = sourceSet.AdditionalRows.Select(x => new ConditionalExpressionRow(x)).ToList();
            InitialConditionalRow = new ConditionalExpressionRow(sourceSet.InitialConditionalRow);
        }

        public ConditionalExpressionSetData Serialize()
        {
            var data = new ConditionalExpressionSetData()
            {
                InitialConditionalRow = InitialConditionalRow?.Serialize(),
                AdditionalRows = AdditionalRows.Select(x => x.Serialize()).ToList()
            };
            data.SetModelData(this);
            return data;
        }
    }
}