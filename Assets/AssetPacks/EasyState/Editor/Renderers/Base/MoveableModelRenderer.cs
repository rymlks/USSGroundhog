using EasyState.Core.Models;
using UnityEngine;
using UnityEngine.UIElements;

namespace EasyState.Editor.Renderers
{
    public abstract class MoveableModelRenderer<T> : ModelRenderer<T> where T : MoveableModel
    {
        public override VisualElement Element { get; protected set; }
        public readonly Design Design;
        private bool _cachedSelected;

        protected MoveableModelRenderer(T moveableModel, VisualElement element, Design design) : base(moveableModel)
        {
            Element = element;
            Design = design;
            Element.RegisterCallback<MouseLeaveEvent>(OnMouseLeave);
            Element.RegisterCallback<MouseDownEvent>(OnMouseDown);
            Element.RegisterCallback<MouseMoveEvent>(OnMouseMove);
            Element.RegisterCallback<MouseUpEvent>(OnMouseUp);
            Element.RegisterCallback<WheelEvent>(OnWheelMoved);
            Model.Changed += Changed;
            Model.Moving += Model_Moving;
            Element.transform.position = moveableModel.Position;
            var doubleClicked = new Clickable(ElementDoubleClicked);
            doubleClicked.activators.Clear();
            doubleClicked.activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse, clickCount = 2 });
            Element.AddManipulator(doubleClicked);
        }

        public override void Dispose()
        {
            Element.UnregisterCallback<MouseDownEvent>(OnMouseDown);
            Element.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
            Element.UnregisterCallback<MouseUpEvent>(OnMouseUp);
            Element.UnregisterCallback<WheelEvent>(OnWheelMoved);
            Element.UnregisterCallback<MouseLeaveEvent>(OnMouseLeave);
            Model.Changed -= Changed;
            Model.Moving -= Model_Moving;
            OnDispose();
        }

        public abstract void OnRightClick(Vector2 screenPoint);

        public abstract void Select();

        public abstract void UnSelect();

        protected virtual void OnDispose()
        {
        }

        protected virtual void OnModelChanged()
        {
        }

        protected virtual void OnModelDoubleCliked()
        {
            Design.OnDetailsPanelRequested(this);
        }

        private void Changed()
        {
            Element.transform.position = Model.Position;
            if (_cachedSelected != Model.Selected)
            {
                _cachedSelected = Model.Selected;
                if (Model.Selected)
                {
                    Select();
                }
                else
                {
                    UnSelect();
                }
            }
            OnModelChanged();
        }

        private void ElementDoubleClicked(EventBase evt)
        {
            evt.StopImmediatePropagation();
            OnModelDoubleCliked();
        }

        private void Model_Moving(MoveableModel obj)
        {
            Element.transform.position = obj.Position;
        }


        private void OnMouseDown(MouseDownEvent x)
        {
            if (x.button != 2)
            {
                Design.OnMouseDown(this, x);
            }
            else
            {
                Design.OnMouseDown(Design, x);
            }
        }

        private void OnMouseLeave(MouseLeaveEvent x) => Design.OnMouseLeft(this, x);

        private void OnMouseMove(MouseMoveEvent x) => Design.OnMouseMove(Model, x);

        private void OnMouseUp(MouseUpEvent x)
        {
            if (x.button == 1)
            {
                x.StopImmediatePropagation();
                OnRightClick(x.mousePosition);
            }
            else if (x.button == 2)
            {
                Design.OnMouseUp(Design, x);
            }
            else
            {
                Design.OnMouseUp(Model, x);
            }
        }

        private void OnWheelMoved(WheelEvent evt) => Design.OnMouseWheelMoved(Model, evt);
    }
}