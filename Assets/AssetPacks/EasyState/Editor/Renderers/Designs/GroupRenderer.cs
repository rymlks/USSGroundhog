using EasyState.Core.Models;
using EasyState.Editor.Renderers.Nodes;
using UnityEngine.UIElements;

namespace EasyState.Editor.Renderers.Designs
{
    internal class GroupRenderer : ModelRenderer<Group>
    {
        public override VisualElement Element { get; protected set; }
        public readonly Design Design;
        private readonly NodeLayerRenderer _nodeLayerRenderer;
        private VisualElement _content;
        private VisualElement _header;
        private bool _highlighted;
        private Label _title;
        private TextField _titleInput;

        public GroupRenderer(Group model, VisualElement groupElement, NodeLayerRenderer nodeLayerRenderer) : base(model)
        {
            Design = nodeLayerRenderer.Model;
            Element = groupElement;
            _header = groupElement.Q<VisualElement>("header");
            _content = groupElement.Q<VisualElement>("group-content");
            _title = _header.Q<Label>();
            var onLabelDoubleClick = new Clickable(OnTitleDoubleClicked);
            onLabelDoubleClick.activators.Clear();
            onLabelDoubleClick.activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse, clickCount = 2 });
            _title.AddManipulator(onLabelDoubleClick);
            _titleInput = _header.Q<TextField>();
            _titleInput.isDelayed = true;
            _titleInput.RegisterValueChangedCallback(OnTitleChanged);
            _titleInput.RegisterCallback<FocusOutEvent>(OnLostFocus);
            _title.text = Model.Name ?? Group.DEFAULT_GROUP_NAME;
            _header.RegisterCallback<MouseDownEvent>(OnMouseDown);
            _header.RegisterCallback<MouseMoveEvent>(OnMouseMove);
            _header.RegisterCallback<MouseUpEvent>(OnMouseUp);
            Element.RegisterCallback<MouseLeaveEvent>(OnMouseLeave);
            Element.RegisterCallback<WheelEvent>(OnWheelMoved);
            Element.RegisterCallback<MouseDownEvent>(OnMouseDownInGroup);
            Element.RegisterCallback<MouseMoveEvent>(OnMouseMoveInGroup);
            Element.RegisterCallback<MouseUpEvent>(OnMouseUpInGroup);
            Model.Changed += Group_Changed;
            Model.ModelRemoved += Group_ModelRemoved;
            Design.MouseMove += Design_MouseMove;
            Design.StateChanged += Design_StateChanged;
            _nodeLayerRenderer = nodeLayerRenderer;
            Group_Changed();
        }

        private void OnLostFocus(FocusOutEvent evt)
        {
            _titleInput.style.display = DisplayStyle.None;
            _title.style.display = DisplayStyle.Flex;
        }

        public override void Dispose()
        {
            Element.UnregisterCallback<MouseLeaveEvent>(OnMouseLeave);
            Element.UnregisterCallback<WheelEvent>(OnWheelMoved);
            Element.UnregisterCallback<MouseDownEvent>(OnMouseDownInGroup);
            Element.UnregisterCallback<MouseMoveEvent>(OnMouseMoveInGroup);
            Element.UnregisterCallback<MouseUpEvent>(OnMouseUpInGroup);
            _header.UnregisterCallback<MouseDownEvent>(OnMouseDown);
            _header.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
            _header.UnregisterCallback<MouseUpEvent>(OnMouseUp);
        }

        private void Design_MouseMove(Model model, MouseMoveEvent evt)
        {
            if (Design.State == DesignState.DraggingItem)
            {
                if (Model.Rect.Contains(Design.GetRelativePosition(evt.mousePosition) - Design.ToolbarOffset))
                {
                    if (model is IGroupable)
                    {
                        var node = model as IGroupable;
                        if (node.Group != Model)
                        {
                            _content.AddToClassList("blue");
                            _highlighted = true;
                        }
                    }
                }
                else
                {
                    _content.RemoveFromClassList("blue");
                    _highlighted = false;
                }
            }
        }

        private void Design_StateChanged(DesignState oldState, DesignState newState)
        {
            if (oldState == DesignState.DraggingItem && _highlighted)
            {
                _content.RemoveFromClassList("blue");

                var nodes = Design.GetSelectedModels();
                foreach (var model in nodes)
                {
                    if (model is IGroupable)
                    {
                        var groupable = model as IGroupable;
                        if (groupable.Group != Model)
                        {
                            Model.AddChild(groupable);
                        }
                    }
                }
                _highlighted = false;
            }
        }

        private void Group_Changed()
        {
            Element.transform.position = Model.Position;
            Element.style.width = Model.Rect.width;
            Element.style.height = Model.Rect.height;
            if (Model.Selected)
            {
                Element.AddToClassList("blue-border-focus");
            }
            else
            {
                Element.RemoveFromClassList("blue-border-focus");
            }
        }

        private void Group_ModelRemoved(IGroupable obj)
        {
            if (Model.Children.Count == 0)
            {
                _nodeLayerRenderer.Model.RemoveGroup(Model);
            }
        }
        private void OnMouseDown(MouseDownEvent x)
        {
            Design.OnMouseDown(Model, x);
            x.StopImmediatePropagation();
        }

        private void OnMouseDownInGroup(MouseDownEvent x) => Design.OnMouseDown(Design, x);

        private void OnMouseLeave(MouseLeaveEvent x) => Design.OnMouseLeft(this, x);

        private void OnMouseMove(MouseMoveEvent x)
        {
            Design.OnMouseMove(Model, x);
            x.StopImmediatePropagation();
        }

        private void OnMouseMoveInGroup(MouseMoveEvent x) => Design.OnMouseMove(Design, x);

        private void OnMouseUp(MouseUpEvent x)
        {
            if (x.button == 1)
            {
                Design.SelectModel(this);
                Design.OnContextMenuRequested(ContextMenuType.Group, x.mousePosition, this);
            }
            else
            {
                Design.OnMouseUp(Model, x);
            }
            x.StopImmediatePropagation();
        }

        private void OnMouseUpInGroup(MouseUpEvent x)
        {
            if (x.button == 1)
            {
                Design.SelectModel(this);
                Design.OnContextMenuRequested(ContextMenuType.Group, x.mousePosition, this);
            }
            else
            {
                Design.OnMouseUp(Design, x);
            }
            x.StopImmediatePropagation();
        }

        private void OnTitleChanged(ChangeEvent<string> evt)
        {
            Model.Name = evt.newValue;
            _titleInput.style.display = DisplayStyle.None;
            if (string.IsNullOrEmpty(evt.newValue))
            {
                _title.text = Group.DEFAULT_GROUP_NAME;
            }
            else
            {
                _title.text = evt.newValue;
            }
            _title.style.display = DisplayStyle.Flex;
        }

        private void OnTitleDoubleClicked()
        {
            _title.style.display = DisplayStyle.None;
            _titleInput.style.display = DisplayStyle.Flex;
            _titleInput.SetValueWithoutNotify(Model.Name);
#if !UNITY_2019
            _titleInput.Focus();
#endif
        }

        private void OnWheelMoved(WheelEvent evt) => Design.OnMouseWheelMoved(Model, evt);
    }
}