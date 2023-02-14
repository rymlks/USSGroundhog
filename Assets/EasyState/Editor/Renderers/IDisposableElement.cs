using System;
using UnityEngine.UIElements;

namespace EasyState.Editor.Renderers
{
    interface IDisposableElement : IDisposable
    {
        VisualElement GetElement();
    }
}
