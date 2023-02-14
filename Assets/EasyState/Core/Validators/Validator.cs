using EasyState.DataModels;
using System.Collections.Generic;
namespace EasyState.Core.Validators
{
    public static class Validator
    {
        private static List<string> EmptySet => new List<string>();

        public static ValidationResult Validate(DesignData design, IEnumerable<DesignDataShort> otherDesigns)
        {
            List<string> errors = EmptySet;
            design.Require().String(errors, x => x.Name);
            design.Require().String(errors, x => x.DataTypeID);
            otherDesigns.Require().Unique(errors, x => x.Name, design.Name);

            return SendResult(errors);
        }

        private static ValidationResult SendResult(List<string> errors)
        {
            if (errors.Count == 0)
            {
                return ValidationResult.ValidResult;
            }
            else
            {
                return new ValidationResult(errors);
            }
        }
    }
}