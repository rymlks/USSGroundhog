using EasyState.Core.Models;
using EasyState.Core.Utility;
using EasyState.Editor.Utility;
using UnityEngine;
using UnityEngine.UIElements;

namespace EasyState.Editor.Renderers.Nodes
{
    public class JumpNodeRenderer : MoveableModelRenderer<Node>
    {
        private Label _nodeTitle;
        private Color _backgroundColor;
        private Color _backgroundSelectedColor;
        private VisualElement _transitionButton;
        private VisualElement _splitIcon;
        private VisualElement _expandButton;
        private VisualElement _nodeSummary;
        private Label _nodeSummaryText;
        private VisualElement _collapseButton;
        public JumpNodeRenderer(Node moveableModel, VisualElement element, Design design) : base(moveableModel, element, design)
        {
            Element.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
            _nodeTitle = Element.Q<Label>("node-name");
            _nodeTitle.text = moveableModel.Name;
            _backgroundColor = Model.NodeColor;
            _backgroundColor.a = .5f;
            _backgroundSelectedColor = Model.SelectedNodeColor;
            _backgroundSelectedColor.a = .5f;
            _transitionButton = Element.Q<VisualElement>("split");
            _splitIcon = Element.Q<VisualElement>("split-icon");
            _transitionButton.RegisterCallback<MouseOverEvent>(x => _splitIcon.style.unityBackgroundImageTintColor = EditorColors.White_Focus);
            _transitionButton.RegisterCallback<MouseOutEvent>(x => _splitIcon.style.unityBackgroundImageTintColor = EditorColors.White);
            _transitionButton.RegisterCallback<MouseDownEvent>(OnJumpToDesignClicked);
            _expandButton = Element.Q<VisualElement>("expand-icon");
            _nodeSummary = Element.Q<VisualElement>("summary-wrapper");
            _nodeSummaryText = _nodeSummary.Q<Label>("summary");
            _collapseButton = Element.Q<VisualElement>("collapse-icon");
            if (Model.JumpDesign is null)
            {
                _transitionButton.style.display = DisplayStyle.None;
            }
            _expandButton.AddManipulator(new Clickable(() =>
            {
                Model.IsSummaryVisible = true;
                OnModelChanged();

            }));
            _collapseButton.AddManipulator(new Clickable(() =>
            {
                Model.IsSummaryVisible = false;
                OnModelChanged();

            }));
            OnModelChanged();
        }

        private void HandleNoSummary()
        {
            _expandButton.parent.style.display = DisplayStyle.None;
            _nodeSummary.style.display = DisplayStyle.None;
        }
        private void HandleSummary()
        {
            _expandButton.parent.style.display = DisplayStyle.Flex;
            _nodeSummaryText.text = Model.NodeSummary;
            if (Model.IsSummaryVisible)
            {
                _collapseButton.style.display = DisplayStyle.Flex;
                _expandButton.style.display = DisplayStyle.None;
                _nodeSummary.style.display = DisplayStyle.Flex;
            }
            else
            {
                _collapseButton.style.display = DisplayStyle.None;
                _expandButton.style.display = DisplayStyle.Flex;
                _nodeSummary.style.display = DisplayStyle.None;
            }

        }
        public override void OnRightClick(Vector2 screenPoint)
        {
            Design.OnContextMenuRequested(ContextMenuType.JumpNode, screenPoint, this);
        }
        protected override void OnModelChanged()
        {
            _nodeTitle.text = Model.Name;
            if (Model.Selected)
            {
                Select();
            }
            else
            {
                UnSelect();
            }
            if (Model.JumpDesign is null)
            {
                _transitionButton.style.display = DisplayStyle.None;
            }
            else
            {
                _transitionButton.style.display = DisplayStyle.Flex;
            }
            if (string.IsNullOrEmpty(Model.NodeSummary))
            {
                HandleNoSummary();
            }
            else
            {
                HandleSummary();
            }
        }
        public override void Select()
        {
            Element.SetBorderColor(Model.SelectedNodeColor);
            Element.style.backgroundColor = _backgroundSelectedColor;

        }

        public override void UnSelect()
        {
            Element.SetBorderColor(Model.NodeColor);
            Element.style.backgroundColor = _backgroundColor;

        }
        private void OnJumpToDesignClicked(MouseDownEvent evt)
        {
            evt.StopImmediatePropagation();
            evt.StopPropagation();
            if (evt.button == 0)
            {
                Design.OnJumpToDesign(Model.JumpDesign.Id, Model.JumpNode.Id);
            }
        }
        protected override void OnDispose()
        {
            Element.UnregisterCallback<GeometryChangedEvent>(OnGeometryChanged);
        }
        private void OnGeometryChanged(GeometryChangedEvent evt)
        {
            Model.Rect = evt.newRect;
        }
    }
}
