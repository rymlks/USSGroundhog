using EasyState.Core.Models;
using UnityEngine.UIElements;

namespace EasyState.Core.Behaviors
{
    public class RectangleSelectBehavior : Behavior
    {

        public RectangleSelectBehavior(Design design) : base(design)
        {
            Design.MouseDown += Design_MouseDown;
            Design.MouseUp += OnMouseUp;
        }

        public override void Dispose()
        {
            Design.MouseDown -= Design_MouseDown;
            Design.MouseUp -= OnMouseUp;
        }

        private void Design_MouseDown(Model model, MouseDownEvent evt)
        {
            if (model?.Id != Design.Id || Design.State == DesignState.CreatingTransition || evt.actionKey || evt.button != 0)
            {
                return;
            }
            Design.State = DesignState.DrawingRectangleSelect;
        }

        private void OnMouseUp(Model model, MouseUpEvent evt)
        {
            if (Design.State == DesignState.DrawingRectangleSelect)
            {
                Design.State = DesignState.Idle;
            }
        }
    }
}