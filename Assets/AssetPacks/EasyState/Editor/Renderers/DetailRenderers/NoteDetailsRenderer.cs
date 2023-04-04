using EasyState.Core.Models;
using UnityEngine.UIElements;

namespace EasyState.Editor.Renderers.DetailRenderers
{
    public class NoteDetailsRenderer : DetailRendererBase<Note>
    {
        protected override string Title => "Edit Note";

        public NoteDetailsRenderer(VisualElement container, Note note, Design design) : base(container, note, design)
        {
        }

        protected override void AddPropertyFields()
        {
            PropertyCollection.AddColorProperty(x => x.NoteColor, "note-color");
            PropertyCollection.AddColorProperty(x => x.TextColor, "text-color");
            PropertyCollection.AddTextProperty(x => x.Contents, "note-input");
            PropertyCollection.AddTextProperty(x => x.Name, "name-input");
        }
    }
}