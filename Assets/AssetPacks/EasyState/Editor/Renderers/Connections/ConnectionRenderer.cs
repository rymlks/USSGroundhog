using EasyState.Core.Models;
using EasyState.Core.Utility;
using EasyState.Editor.Utility;
using UnityEngine;
using UnityEngine.UIElements;

namespace EasyState.Editor.Renderers.Connections
{
    internal class ConnectionRenderer : MoveableModelRenderer<Connection>
    {
        public override VisualElement Element { get; protected set; }
        private readonly float _connectionPointOffset;
        private ConnectionLayerRenderer _layerRenderer;
        private VisualElement _lineElement;

        public ConnectionRenderer(Connection model, VisualElement handleElement, ConnectionLayerRenderer layerRenderer) : base(model, handleElement, layerRenderer)
        {
            _layerRenderer = layerRenderer;
            Element.name = "conn-" + model.Id;
            Element.style.position = Position.Absolute;
            Element.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
            Element.style.width = _layerRenderer.ConnectionSettings.HandleSize * 1.4f;
            Element.style.height = _layerRenderer.ConnectionSettings.HandleSize * 1.4f;
            Element.transform.position = Model.Position;
            Model.Changed += Model_Changed;
            _lineElement = new VisualElement();
            _layerRenderer.Element.Add(_lineElement);
            _lineElement.generateVisualContent += DrawConnectionLine;
            _connectionPointOffset = layerRenderer.ConnectionSettings.InputOutputOffset;
            if (model.IsFallback)
            {
                _connectionPointOffset += layerRenderer.ConnectionSettings.FallbackConnectionOffset;
            }
            Element.generateVisualContent += OnDrawTriangle;
        }

        public override void Dispose()
        {
            _lineElement.generateVisualContent -= DrawConnectionLine;
            _layerRenderer.Element.Remove(_lineElement);
            Model.Changed -= Model_Changed;
            Element.UnregisterCallback<GeometryChangedEvent>(OnGeometryChanged);
            Element.generateVisualContent -= OnDrawTriangle;
        }

        public override void OnRightClick(Vector2 screenPoint) => Design.OnContextMenuRequested(ContextMenuType.Connection, screenPoint, Model);

        public void Refresh() => Model_Changed();

        public override void Select() { }

        public override void UnSelect() { }

        private void DrawConnectionLine(MeshGenerationContext context)
        {
            if (!Model.CanDraw)
            {
                return;
            }
            if (Model.DestNode == null)
            {
                return;
            }

            Vector2 a = ConnectionPointUtility.CalculateOutputPosition(Model.SourceNode.Rect, Model.Rect, _connectionPointOffset);
            Vector2 b = Model.Rect.center;
            Vector2 c = ConnectionPointUtility.CalculateInputPosition(Model.DestNode.Rect, Model.Rect, _connectionPointOffset); ;
            Color lineColor;

            if (Model.IsFallback)
            {
                lineColor = Model.Selected? _layerRenderer.ConnectionSettings.SelectedFallbackLineColor :  _layerRenderer.ConnectionSettings.FallbackLineColor;
            }
            else
            {
                lineColor = Model.Selected? _layerRenderer.ConnectionSettings.SelectedLineColor : _layerRenderer.ConnectionSettings.LineColor;
            }

            RendererUtility.DrawLine(new Vector3[] { a, b, c }, _layerRenderer.ConnectionSettings.ConnectionLineWidth, lineColor, context);
        }

        private void Model_Changed()
        {
            Element.transform.position = Model.Position;
            Element.tooltip = Model.IsFallback ? "Fallback connection" : Model.Name;
            _lineElement.MarkDirtyRepaint();
            Element.MarkDirtyRepaint();
        }

        private void OnDrawTriangle(MeshGenerationContext context)
        {
            if (!Model.CanDraw)
            {
                return;
            }
            Vector3 destPos = ConnectionPointUtility.CalculateInputPosition(Model.DestNode.Rect, Model.Rect, _connectionPointOffset);
            Vector3 triangleBase = Model.Rect.center - Model.Rect.position;
            Vector3 dir = ((destPos - (Vector3)Model.Rect.position) - triangleBase).normalized;
            Vector3 perp = Vector2.Perpendicular(dir);
            float scalar = _layerRenderer.ConnectionSettings.HandleSize;
            var a = (triangleBase + -dir + perp * scalar) + dir;
            var b = (triangleBase + -dir - perp * scalar) + dir;
            var c = (triangleBase + dir) + dir * scalar * 1.3f;
            float offset = 4f;
            a -= dir * offset;
            b -= dir * offset;
            c -= dir * offset;
            a.z = Vertex.nearZ;
            b.z = Vertex.nearZ;
            c.z = Vertex.nearZ;
            var mesh = context.Allocate(3, 3);
            Color meshColor;
            if (Model.Selected)
            {
                if (Model.IsFallback)
                {
                    meshColor = _layerRenderer.ConnectionSettings.SelectedFallbackLineColor;
                }
                else
                {
                    meshColor = _layerRenderer.ConnectionSettings.SelectedLineColor;
                }
            }
            else
            {
                if (Model.IsFallback)
                {
                    meshColor = _layerRenderer.ConnectionSettings.FallbackLineColor;
                }
                else
                {
                    meshColor = _layerRenderer.ConnectionSettings.LineColor;
                }
            }
            mesh.SetAllVertices(new Vertex[] {
                new Vertex{
                    position = a,
                    tint = meshColor
                },
                 new Vertex{
                    position = b,
                    tint = meshColor
                },
                  new Vertex{
                    position = c,
                    tint = meshColor
                },
            });
            mesh.SetAllIndices(new ushort[] { 0, 1, 2 });
        }

        private void OnGeometryChanged(GeometryChangedEvent evt)
        {
            Model.Rect = evt.newRect;
            Model.Refresh();
            Model_Changed();
        }
    }
}