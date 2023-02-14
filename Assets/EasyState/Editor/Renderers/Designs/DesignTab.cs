using EasyState.Core.Models;
using UnityEngine.UIElements;

namespace EasyState.Editor.Renderers.Designs
{
    public class DesignTab : Tab
    {
        public Design Design { get; private set; }
        protected readonly VisualElement closeBtn;
        public DesignTab(VisualElement tabElement, TabContainer container, Design design, int tabIndex) : base(tabElement, container, tabIndex)
        {
            Design = design;
            tabLabel.text = Design.Name ?? "Unnamed Design";
            closeBtn = tabElement.Q<VisualElement>("close-tab");
            closeBtn.RegisterCallback<MouseDownEvent>(OnClose);
        }
        public void SetText(string newText)=> tabLabel.text = newText;
        public override void Dispose()
        {
            base.Dispose();
            closeBtn.UnregisterCallback<MouseDownEvent>(OnClose);
        }

        public override void DeSelect()
        {
            base.DeSelect();
            Design.IsVisible = false;
        }
        public override void Select()
        {
            base.Select();
            Design.IsVisible = true;
        }
        protected void OnClose(MouseDownEvent evt)
        {
            container.OnDesignClosed(this);
            Design.IsVisible = false;
        }

        protected override void OnTabClicked(MouseDownEvent evt)
        {
            container.OnTabClicked(this);
        }

    }
}