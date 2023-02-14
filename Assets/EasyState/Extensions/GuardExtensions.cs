using System;

namespace EasyState.Extensions
{
    public static class GuardExtensions
    {
        public static void Guard(this object obj, string message = "this should not be null.")
        {
            if (obj == null)
            {
                throw new NullReferenceException(message);
            }
        }

        public static void GuardNullArg(this object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
        }
    }
}