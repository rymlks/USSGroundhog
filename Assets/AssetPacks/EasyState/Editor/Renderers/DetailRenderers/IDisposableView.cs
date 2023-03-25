using System;

namespace EasyState.Editor.Renderers.DetailRenderers
{
    public interface IDisposableView : IDisposable
    {
        void Refresh();
    }
}