using UnityEngine.UIElements;

namespace EasyState.Editor.Renderers.DetailRenderers
{
    public class DetailRendererResponse
    {
        public VisualElement Contents { get; set; }

        public string Title { get; set; }

        public DetailRendererResponse(string title, VisualElement contents)
        {
            Title = title;
            Contents = contents;
        }
    }
}