using EasyState.Core.Models;
using EasyState.Editor.Templates;
using UnityEngine.UIElements;

namespace EasyState.Editor.Renderers.Nodes
{
    public class NoteLayerRenderer : LayerRenderer<Note>
    {
        public NoteLayerRenderer(Design design, VisualElement container) : base(design, container)
        {
        }

        protected override ModelRenderer<Note> BuildRenderer(Note model)
        {
            var template = TemplateFactory.CreateNoteTemplate();

            var renderer = new NoteRenderer(model, template, Design);

            return renderer;
        }
    }
}