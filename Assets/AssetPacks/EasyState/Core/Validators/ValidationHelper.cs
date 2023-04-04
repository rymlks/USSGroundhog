using System.Text.RegularExpressions;

namespace EasyState.Core.Validators
{
    public static class ValidationHelper
    {
        public const string SafeClassNamePattern = "([^a-zA-z0-9_$])";
        private static readonly Regex _validStringRegex = new Regex(SafeClassNamePattern);

        /// <summary>
        /// Tests to make sure strings do no contain symbols or spaces
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsSafeClassString(string value) => _validStringRegex.IsMatch(value);
    }
}