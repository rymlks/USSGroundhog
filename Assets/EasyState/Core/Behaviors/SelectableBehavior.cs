using EasyState.Core.Models;
using UnityEngine.UIElements;

namespace EasyState.Core.Behaviors
{
    internal class SelectableBehavior : Behavior
    {
        public SelectableBehavior(Design design) : base(design)
        {
            Design.MouseDown += Design_MouseDown;
        }

        public override void Dispose()
        {
            Design.MouseDown -= Design_MouseDown;
        }

        private void Design_MouseDown(Model model, MouseDownEvent evt)
        {

            if (!(model is SelectableModel) && evt.button == 0)
            {
                Design.UnselectAll();
            }
            else if (evt.button == 0)
            {
                Design.SelectModel(model as SelectableModel, !evt.actionKey);
            }
        }
    }
}