using EasyState.Core.Models;
using EasyState.Core.Utility;
using EasyState.Editor.Utility;
using EasyState.Settings;
using UnityEngine;
using UnityEngine.UIElements;

namespace EasyState.Editor.Renderers.Nodes
{
    public class NodeRenderer : MoveableModelRenderer<Node>
    {
        private Label _nodeTitle;
        private VisualElement _splitIcon;
        private VisualElement _transitionButton;
        private VisualElement _collapseButton;
        private VisualElement _expandButton;
        private VisualElement _nodeSummary;
        private Label _nodeSummaryText;
        public NodeRenderer(Node model, Design design, VisualElement nodeTemplate) : base(model, nodeTemplate, design)
        {
            Element.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
            _nodeTitle = Element.Q<Label>("node-name");
            _nodeTitle.text = model.Name;
            _transitionButton = Element.Q<VisualElement>("split");
            _splitIcon = Element.Q<VisualElement>("split-icon");
            _collapseButton = Element.Q<VisualElement>("collapse-icon");
            _expandButton = Element.Q<VisualElement>("expand-icon");
            _nodeSummary = Element.Q<VisualElement>("summary-wrapper");
            _nodeSummaryText = _nodeSummary.Q<Label>("summary");

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

            _transitionButton.tooltip = "Create Connection";
            _transitionButton.RegisterCallback<MouseOverEvent>(x =>
            {
                _transitionButton.SetBorderColor(EditorColors.White_Focus);
                _splitIcon.style.unityBackgroundImageTintColor = EditorColors.White_Focus;
            });

            _transitionButton.RegisterCallback<MouseOutEvent>(x =>
            {
                _transitionButton.SetBorderColor(EditorColors.White);
                _splitIcon.style.unityBackgroundImageTintColor = EditorColors.White;
            });
            _transitionButton.RegisterCallback<MouseDownEvent>(OnCreateConnectionClicked);
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
            Design.OnContextMenuRequested(ContextMenuType.Node, screenPoint, this);
        }

        public override void Select()
        {
            Element.SetBorderColor(Model.SelectedNodeColor);
        }

        public override void UnSelect()
        {
            Element.SetBorderColor(Model.NodeColor);
        }

        protected override void OnDispose()
        {
            Element.UnregisterCallback<GeometryChangedEvent>(OnGeometryChanged);
        }

        protected override void OnModelChanged()
        {
            if (!Model.IsEntryNode && Model.Name == NodePresetCollection.ENTRY_NODE)
            {
                Model.Name = string.Empty;
                Design.OnShowToast("Can't name node 'Entry State'", 2000);
            }
            _nodeTitle.text = Model.Name;
            if (Model.Selected)
            {
                Select();
            }
            else
            {
                UnSelect();
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

        private void OnCreateConnectionClicked(MouseDownEvent evt)
        {
            evt.StopImmediatePropagation();
            evt.StopPropagation();
            if (evt.button == 0)
            {
                Design.OnCreateConnection(Model);
            }
        }

        private void OnGeometryChanged(GeometryChangedEvent evt)
        {
            Model.Rect = evt.newRect;
            if (Model.Group != null)
            {
                Model.Group.Refresh();
            }
            Model.RefreshConnections();
        }


    }
}