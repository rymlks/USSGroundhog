using EasyState.Core.Layers;
using EasyState.Core.Models;
using EasyState.Core.Utility;
using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace EasyState.Editor.Renderers
{
    public abstract class LayerRenderer<T> : IDisposable where T : Model
    {
        public readonly VisualElement Container;
        public readonly Design Design;
        public readonly Dictionary<T, ModelRenderer<T>> RenderRegistry = new Dictionary<T, ModelRenderer<T>>();

        private BaseLayer<T> _layer;

        public LayerRenderer(Design design, VisualElement container)
        {
            Design = design;
            Container = container;
            _layer = Design.Layer<T>();
            _layer.Added += OnItemAddedToLayer;
            _layer.Removed += OnItemRemovedFromLayer;

            foreach (var item in _layer)
            {
                OnItemAddedToLayer(item);
            }
        }

        public void Dispose()
        {
            _layer.Added -= OnItemAddedToLayer;
            _layer.Removed -= OnItemRemovedFromLayer;
            foreach (var item in RenderRegistry)
            {
                item.Value.Dispose();
            }
        }

        protected abstract ModelRenderer<T> BuildRenderer(T model);

        protected void OnItemAddedToLayer(T model)
        {
            var newRenderer = BuildRenderer(model);
            RenderRegistry[model] = newRenderer;
            Container.Add(newRenderer.Element);
        }

        private void OnItemRemovedFromLayer(T model)
        {
            var itemToRemove = RenderRegistry[model];
            itemToRemove.Dispose();
            itemToRemove.Element.RemoveFromHierarchy();
            RenderRegistry.Remove(model);
        }
    }
}