using EasyState.Core.Models;
using UnityEngine.UIElements;

namespace EasyState.Core.Behaviors
{
    internal class ContextMenuBehavior : Behavior
    {
        public ContextMenuBehavior(Design design) : base(design)
        {
            Design.RightMouseUp += Design_RightMouseUp;
        }

        public override void Dispose()
        {
            Design.RightMouseUp -= Design_RightMouseUp;
        }

        private void Design_RightMouseUp(Model model, MouseUpEvent evt)
        {
            if (Design.State == DesignState.CreatingTransition)
            {
                return;
            }
            if (model.Id == Design.Id)
            {
                Design.OnContextMenuRequested(ContextMenuType.Background, evt.mousePosition);
                Design.State = DesignState.ContextMenuOpen;
            }
        }
    }
}