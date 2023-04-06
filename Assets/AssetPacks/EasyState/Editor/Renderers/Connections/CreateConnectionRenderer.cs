using EasyState.Core.Models;
using EasyState.Editor.Utility;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace EasyState.Editor.Renderers.Connections
{
    public class CreateConnectionRenderer : ModelRenderer<Design>, IDisposable
    {
        public override VisualElement Element { get; protected set; }
        private const float LINE_WIDTH = 3f;
        private static readonly Vector3 _nearZ = Vector3.forward * Vertex.nearZ;
        private Vector3 _mousePos;
        private Node _sourceNode;
        private bool _isVisible;
        public CreateConnectionRenderer(Design model, VisualElement connectionContainer) : base(model)
        {
            Element = connectionContainer;
            Element.generateVisualContent += OnGenerateVisualContent;
            Model.CreateConnection += Design_CreateConnection;
            Model.MouseMove += Design_MouseMove;
            Model.StateChanged += Design_StateChanged;
        }

        public override void Dispose()
        {
            Element.generateVisualContent -= OnGenerateVisualContent;
            Model.CreateConnection -= Design_CreateConnection;
            Model.StateChanged -= Design_StateChanged;
        }

        private void Design_CreateConnection(Node node)
        {
            _sourceNode = node;          
            _mousePos = (Vector3)Element.WorldToLocal(Model.MousePosition) + _nearZ;
            Element.MarkDirtyRepaint();
        }

        private void Design_StateChanged(DesignState oldState, DesignState newState)
        {
            if(newState == DesignState.CreatingTransition)
            {
                _isVisible = true;
            }
            if(oldState == DesignState.CreatingTransition)
            {
                _isVisible = false;
                Element.MarkDirtyRepaint();
            }
        }

        private void Design_MouseMove(Model model, MouseMoveEvent evt)
        {
            if (_isVisible)
            {
                _mousePos = (Vector3)Element.WorldToLocal(evt.mousePosition) + _nearZ;
                Element.MarkDirtyRepaint();
            }
        } 

        private void OnGenerateVisualContent(MeshGenerationContext context)
        {
            if (_isVisible)
            {
                Vector3 portPos = _sourceNode.Rect.center; // Model.GetAbsolutePosition(_sourceNode.Rect.center);
                //int stepCount = 31;
                //int curveScalar = 400;
                //Vector3 stepAmount = (_mousePos - portPos) / stepCount;
                //Vector3[] points = new Vector3[stepCount + 1];
                //AnimationCurve curve = EasyState.Settings.EasyStateSettings.Instance.ConnectionSettings.UnsetConnectionCurve;
                //points[0] = portPos;
                //for (int i = 1; i < stepCount; i++)
                //{
                //    Vector3 p = portPos + (i * stepAmount);
                //    float percentage = i / ((float)stepCount);
                //    float curveSample = curve.Evaluate(percentage);
                //    p.y += curveScalar * curveSample;
                //    points[i] = p;
                //}
                //points[stepCount] = _mousePos;
                //RendererUtility.DrawLine(points, LINE_WIDTH, EditorColors.Green_Focus, context);
                RendererUtility.DrawCurvedDashedLine(new RendererUtility.DashedLineRequest(portPos, _mousePos, context));
            }
          
        }
    }
}