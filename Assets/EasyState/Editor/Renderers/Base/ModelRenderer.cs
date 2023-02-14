using EasyState.Core.Models;
using UnityEngine.UIElements;

namespace EasyState.Editor.Renderers
{
    public abstract class ModelRenderer<T> : IDisposableElement where T : Model
    {
        public abstract VisualElement Element { get; protected set; }
        public T Model { get; }

        public ModelRenderer(T model)
        {
            Model = model;
        }

        public static implicit operator T(ModelRenderer<T> renderer) => renderer.Model;

        public static implicit operator VisualElement(ModelRenderer<T> renderer) => renderer.Element;

        public abstract void Dispose();

        public VisualElement GetElement() => Element;
    }
}