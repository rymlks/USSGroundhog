using System;
using System.Collections.Generic;

namespace EasyState.Extensions
{
    public static class DisposableExtension
    {
        public static void DisposeAll(this List<IDisposable> disposables)
        {
            if (disposables == null)
            {
                return;
            }
            if (disposables.Count == 0)
            {
                return;
            }
            foreach (var disposeable in disposables)
            {
                disposeable.Dispose();
            }
        }
    }
}