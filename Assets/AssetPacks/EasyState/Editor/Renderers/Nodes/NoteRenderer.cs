using EasyState.Core.Models;
using UnityEngine;
using UnityEngine.UIElements;

namespace EasyState.Editor.Renderers.Nodes
{
    internal class NoteRenderer : MoveableModelRenderer<Note>
    {
        private Label _contents;
        private Label _title;

        public NoteRenderer(Note moveableModel, VisualElement element, Design design) : base(moveableModel, element, design)
        {
            _title = element.Q<Label>("title");
            _contents = element.Q<Label>("content");
            Element.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
            OnModelChanged();
        }

        public override void OnRightClick(Vector2 screenPoint)
        {
            Design.OnContextMenuRequested(ContextMenuType.Note, screenPoint, this);
        }

        public override void Select()
        {
            Element.AddToClassList("blue-border-focus");
            Element.AddToClassList("border");
        }

        public override void UnSelect()
        {
            Element.RemoveFromClassList("blue-border-focus");
            Element.RemoveFromClassList("border");
        }
        private void OnGeometryChanged(GeometryChangedEvent evt)
        {
            Model.Rect = evt.newRect;
            if(Model.Group != null)
            {
                Model.Group.Refresh();
            }
        }
        protected override void OnModelChanged()
        {
            _title.text = Model.Name ?? Note.DEFAULT_TITLE;
            _contents.text = Model.Contents ?? Note.DEFAULT_NOTE;
            Element.style.backgroundColor = Model.NoteColor;
            _title.style.color = Model.TextColor;
            _contents.style.color = Model.TextColor;
        }
    }
}