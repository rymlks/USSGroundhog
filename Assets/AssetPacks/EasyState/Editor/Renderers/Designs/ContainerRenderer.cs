using EasyState.Core.Models;
using System;
using UnityEngine.UIElements;

namespace EasyState.Editor.Renderers.Designs
{
    public class ContainerRenderer : ModelRenderer<Design>, IDisposable
    {
        public override VisualElement Element { get; protected set; }

        public ContainerRenderer(Design model, VisualElement container) : base(model)
        {
            Element = container;
            Model.PanChanged += OnContainerPanUpdated;
            Model.ZoomChanged += OnContainerZoomUpdated;
            OnContainerPanUpdated();
            OnContainerZoomUpdated();
        }

        public override void Dispose()
        {
            Model.PanChanged -= OnContainerPanUpdated;
        }

        private void OnContainerPanUpdated()
        {
            Element.transform.position = Model.Pan;
        }

        private void OnContainerZoomUpdated()
        {
            Element.transform.scale = new UnityEngine.Vector3(Model.Zoom, Model.Zoom, Model.Zoom);
        }
    }
}