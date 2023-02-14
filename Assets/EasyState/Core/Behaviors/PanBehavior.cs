using EasyState.Core.Models;
using UnityEngine.UIElements;

namespace EasyState.Core.Behaviors
{
    public class PanBehavior : Behavior
    {
        public PanBehavior(Design design) : base(design)
        {
            Design.MouseDown += OnMouseDown;
            Design.MouseMove += OnMouseMove;
            Design.MouseUp += OnMouseUp;
        }

        public override void Dispose()
        {
            Design.MouseDown -= OnMouseDown;
            Design.MouseMove -= OnMouseMove;
            Design.MouseUp -= OnMouseUp;
        }

        private void OnMouseDown(Model model, MouseDownEvent evt)
        {
            if (model?.Id != Design.Id || Design.State == DesignState.CreatingTransition)
            {
                return;
            }
            if (evt.actionKey || evt.button == 2)
            {
                Design.State = DesignState.Panning;
            }
        }

        private void OnMouseMove(Model model, MouseMoveEvent evt)
        {
            if (Design.State != DesignState.Panning)
            {
                return;
            }
            Design.UpdatePan(evt.mouseDelta);
        }

        private void OnMouseUp(Model model, MouseUpEvent evt)
        {
            if (Design.State == DesignState.Panning)
            {
                Design.State = DesignState.Idle;
            }
        }
    }
}