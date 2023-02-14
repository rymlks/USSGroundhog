using EasyState.Core.Models;
using EasyState.Editor.Renderers.Connections;
using EasyState.Editor.Renderers.Nodes;
using UnityEngine.UIElements;

namespace EasyState.Editor.Renderers.Designs
{
    public class DesignRenderer : ModelRenderer<Design>
    {
        public BackgroundRenderer BackgroundRenderer { get; private set; }
        public ConnectionLayerRenderer ConnectionLayerRenderer { get; private set; }
        public ContainerRenderer ContainerRenderer { get; private set; }
        public CreateConnectionRenderer CreateConnectionRenderer { get; private set; }
        // public DebugRenderer DebugRenderer { get; private set; }
        public override VisualElement Element { get; protected set; }
        public NodeLayerRenderer NodeLayerRenderer { get; private set; }
        public NoteLayerRenderer NoteLayerRenderer { get; private set; }
        public ValidationResultRenderer ValidationResultRenderer { get; private set; }
        private const string BACKGROUND_ELEMENT_ID = "background";
        private const string CONNECTIONS_ELEMENT_ID = "connections";
        private const string CONTAINER_ELEMENT_ID = "container";
        private const string CONTENT_ELEMENT_ID = "content";
        private const string DETAILS_PANEL_ID = "details-panel";
        private const string NODES_ELEMENT_ID = "nodes";
        private const string NOTES_ELEMENT_ID = "notes";
        private const string RECTANGLE_SELECTOR_ID = "rectangle-selector";
        private readonly RectangleSelectorRenderer _rectangleSelectorRenderer;
        private DetailsPanelRenderer _detailsRenderer;


        public DesignRenderer(Design model, VisualElement tabRootElement, VisualElement windowRootElement, bool isDebug = false) : base(model)
        {
            Element = tabRootElement;
            var background = tabRootElement.Q<VisualElement>(BACKGROUND_ELEMENT_ID);
            var container = tabRootElement.Q<VisualElement>(CONTAINER_ELEMENT_ID);
            BackgroundRenderer = new BackgroundRenderer(model, background);
            ContainerRenderer = new ContainerRenderer(model, container);
            NodeLayerRenderer = new NodeLayerRenderer(model, container.Q<VisualElement>(NODES_ELEMENT_ID));
            ConnectionLayerRenderer = new ConnectionLayerRenderer(model, container.Q<VisualElement>(CONNECTIONS_ELEMENT_ID));
            NoteLayerRenderer = new NoteLayerRenderer(model, container.Q<VisualElement>(NOTES_ELEMENT_ID));
            // DebugRenderer = new DebugRenderer(model, tabRootElement);
            if (!isDebug)
            {
                CreateConnectionRenderer = new CreateConnectionRenderer(model, tabRootElement.Q<VisualElement>(CONNECTIONS_ELEMENT_ID));
                _rectangleSelectorRenderer = new RectangleSelectorRenderer(model, windowRootElement.Q<VisualElement>(RECTANGLE_SELECTOR_ID));
                _detailsRenderer = new DetailsPanelRenderer(model, windowRootElement.Q<VisualElement>(DETAILS_PANEL_ID));
                ValidationResultRenderer = new ValidationResultRenderer(model, windowRootElement.Q<VisualElement>(CONTENT_ELEMENT_ID));
            }
            Element.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
        }

        private void OnGeometryChanged(GeometryChangedEvent evt)
        {
            Model.Refresh();
        }

        public override void Dispose()
        {
            BackgroundRenderer.Dispose();
            ContainerRenderer.Dispose();
            NodeLayerRenderer.Dispose();
            ConnectionLayerRenderer.Dispose();
            //  DebugRenderer.Dispose();      
            CreateConnectionRenderer?.Dispose();
            _rectangleSelectorRenderer?.Dispose();
            _detailsRenderer?.Dispose();
            ValidationResultRenderer?.Dispose();
        }

        public void Hide()
        {
            Element.style.display = DisplayStyle.None;
            _detailsRenderer.HideWindow();
        }

        public void Show()
        {
            Element.style.display = DisplayStyle.Flex;

        }
    }
}