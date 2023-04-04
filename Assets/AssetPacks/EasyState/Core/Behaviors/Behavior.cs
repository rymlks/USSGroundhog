using EasyState.Core.Models;
using System;

namespace EasyState.Core.Behaviors
{
    public abstract class Behavior : IDisposable
    {
        public Design Design { get; }

        protected Behavior(Design design)
        {
            Design = design;
        }

        public abstract void Dispose();
    }
}