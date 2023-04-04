using System.Collections.Generic;

namespace EasyState.Core.Validators
{
    public class DesignValidatorResult : ValidationResult
    {
        public List<string> AdditionalDesignsToValidate { get; set; } = new List<string>();
        public DesignValidatorResult(List<string> errors, List<string> addiitonalDesignIds = null) : base(errors)
        {
            if (addiitonalDesignIds != null)
            {
                AdditionalDesignsToValidate = addiitonalDesignIds;
            }
        }
    }
}
