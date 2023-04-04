using EasyState.Core.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace EasyState.Core.Behaviors
{
    internal class DragMoveableBehavior : Behavior
    {
        private bool _isNode;
        private bool _isConnection;
        private IEnumerable<MovementData> _movementData;

        public DragMoveableBehavior(Design design) : base(design)
        {
            Design.MouseDown += OnMouseDown;
            Design.MouseMove += OnMouseMove;
            Design.MouseUp += OnMouseUp;
        }

        public override void Dispose()
        {
            Design.MouseDown -= OnMouseDown;
            Design.MouseMove -= OnMouseMove;
            Design.MouseUp -= OnMouseUp;
        }

        private void MoveModel(MovementData data, Vector2 mousePosition, Vector2 mouseDelta)
        {
            bool shouldSnap = (!_isConnection && Design.Settings.SnapToGrid) || (_isConnection && Design.Settings.ConnectionsSnapToGrid);
            if (shouldSnap)
            {
                //snap movement to grid
                var step = Design.Settings.SnapToGridStep;
                Vector2 newPoint = Design.GetRelativePosition(mousePosition) - data.OrigOffset;

                int stepsX = Mathf.RoundToInt(newPoint.x / step);
                int stepsY = Mathf.RoundToInt(newPoint.y / step);

                Vector2 newPos = new Vector2(step * stepsX, step * stepsY);
                Design.OnDebugMessage(this, "Set pos " + newPos.ToString());
                data.Model.SetPosition(newPos);
            }
            else
            {
                data.Model.UpdatePosition(mouseDelta / Design.Zoom);
            }
        }

        private void OnMouseDown(Model model, MouseDownEvent evt)
        {
            if (model?.Id == Design.Id || Design.State == DesignState.CreatingTransition || evt.button != 0)
            {
                return;
            }
            if (!(model is MoveableModel))
            {
                return;
            }
            _isNode = model is Node;
            _isConnection = model is Connection;
            var modelToMove = model as MoveableModel;
            if (_isNode)
            {
                _movementData = Design.GetSelectedNodes().Select(x => new MovementData { Model = x, OrigOffset = (Design.GetRelativePosition(evt.mousePosition) - x.Position) }).ToList();
            }
            else
            {
                _movementData = new List<MovementData>() { new MovementData(modelToMove, (Design.GetRelativePosition(evt.mousePosition) - modelToMove.Position)) };
            }
            Design.State = DesignState.DraggingItem;
        }

        private void OnMouseMove(Model model, MouseMoveEvent evt)
        {
            if (Design.State != DesignState.DraggingItem)
            {
                return;
            }
            foreach (var item in _movementData)
            {
                MoveModel(item, evt.mousePosition, evt.mouseDelta);
            }
        }

        private void OnMouseUp(Model model, MouseUpEvent evt)
        {
            if (Design.State == DesignState.DraggingItem)
            {
                _movementData = null;
                Design.State = DesignState.Idle;
            }
        }

        private struct MovementData
        {
            public MoveableModel Model { get; set; }

            public Vector2 OrigOffset { get; set; }

            public MovementData(MoveableModel model, Vector2 origOffset)
            {
                Model = model;
                OrigOffset = origOffset;
            }
        }
    }
}