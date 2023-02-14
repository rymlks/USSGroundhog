using System.Collections.Generic;

namespace EasyState.Core.Validators
{
    public class ValidationResult
    {
        public List<string> Errors { get; private set; } = new List<string>();
        public bool IsValid => Errors.Count == 0;

        public static readonly ValidationResult ValidResult = new ValidationResult();

        public ValidationResult(List<string> errors)
        {
            Errors = errors;
        }

        protected ValidationResult()
        { }

        public static implicit operator bool(ValidationResult result) => result.IsValid;

        public string GetErrorMessage()
        {
            if (IsValid)
            {
                return string.Empty;
            }
            string errorMessage = "Errors Found:\n";

            foreach (var error in Errors)
            {
                errorMessage += $"• {error}\n";
            }
            return errorMessage;
        }
    }
}