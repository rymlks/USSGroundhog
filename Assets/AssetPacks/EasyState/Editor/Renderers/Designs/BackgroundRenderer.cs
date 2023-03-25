using EasyState.Core.Models;
using EasyState.Settings;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace EasyState.Editor.Renderers.Designs
{
    public class BackgroundRenderer : ModelRenderer<Design>, IDisposable
    {
        public VisualElement Canvas { get; }

        public override VisualElement Element { get; protected set; }

        public BackgroundRenderer(Design model, VisualElement background) : base(model)
        {
            Element = background;
            Element.RegisterCallback<MouseDownEvent>(OnMouseDown);
            Element.RegisterCallback<MouseMoveEvent>(OnMouseMove);
            Element.RegisterCallback<MouseUpEvent>(OnMouseUp);
            Element.RegisterCallback<WheelEvent>(OnWheelEvent);
            Element.RegisterCallback<MouseLeaveEvent>(OnMouseLeft);
            Element.style.backgroundColor = EasyStateSettings.Instance.BackgroundColor;
            Model.MouseLeft += OnMouseLeave;
        }

        public override void Dispose()
        {
            Element.UnregisterCallback<MouseDownEvent>(OnMouseDown);
            Element.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
            Element.UnregisterCallback<MouseUpEvent>(OnMouseUp);
            Element.UnregisterCallback<WheelEvent>(OnWheelEvent);
            Element.UnregisterCallback<MouseLeaveEvent>(OnMouseLeft);
            Model.MouseLeft -= OnMouseLeave;
        }


        private void OnMouseDown(MouseDownEvent x)
        {
            if (x.button == 0 || x.button == 2)
                Model.OnMouseDown(this, x);
            else if (x.button == 1)
            {
                Model.OnRightMouseDown(this, x);
                x.StopImmediatePropagation();
            }
        }

        private void OnMouseLeave(Model model, MouseLeaveEvent x)
        {
            var pos = Element.WorldToLocal(Event.current.mousePosition);
            bool outsideOfFrame = pos.x < 0 || pos.y < 0 || pos.x + 5 > Element.layout.width || pos.y + 5 > Element.layout.height;
            if (outsideOfFrame)
            {
                if (Model.State != DesignState.Idle)
                {
                    Model.OnDebugMessage(this, "Lost Mouse : " + pos.ToString());
                    Model.OnMouseUp(this, MouseUpEvent.GetPooled(x));
                }
            }
        }

        private void OnMouseLeft(MouseLeaveEvent x) => Model.OnMouseLeft(this, x);

        private void OnMouseMove(MouseMoveEvent x) => Model.OnMouseMove(Model, x);

        private void OnMouseUp(MouseUpEvent x)
        {
            if (x.button == 0 || x.button == 2)
                Model.OnMouseUp(this, x);
            else if (x.button == 1)
            {
                Model.OnRightMouseUp(this, x);
                x.StopImmediatePropagation();
            }
        }

        private void OnWheelEvent(WheelEvent evt) => Model.OnMouseWheelMoved(this, evt);
    }
}