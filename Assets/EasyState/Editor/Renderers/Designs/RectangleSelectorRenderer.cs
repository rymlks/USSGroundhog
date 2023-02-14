using EasyState.Core.Models;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace EasyState.Editor.Renderers.Designs
{
    public class RectangleSelectorRenderer : IDisposable
    {
        private enum Direction { NorthWest, NorthEast, SouthEast, SouthWest }

        private const float SELECTION_PADDING = 5;
        private const float Y_OFFSET = -20;
        private readonly Design _design;
        private readonly VisualElement _selectionElement;
        private Vector2 _startingPoint;

        public RectangleSelectorRenderer(Design design, VisualElement selectionElement)
        {
            _design = design;
            _selectionElement = selectionElement;
            _selectionElement.style.display = DisplayStyle.None;
            _design.StateChanged += Design_StateChanged;
            _design.MouseMove += Design_MouseMove;
        }

        public void Dispose()
        {
            _design.StateChanged -= Design_StateChanged;
            _design.MouseMove -= Design_MouseMove;
        }

        private void Design_MouseMove(Model model, UnityEngine.UIElements.MouseMoveEvent evt)
        {
            if (_design.State == DesignState.DrawingRectangleSelect)
            {
                float width = Mathf.Abs(evt.mousePosition.x - _startingPoint.x);
                float height = Mathf.Abs(evt.mousePosition.y - _startingPoint.y);
                _selectionElement.style.width = width;
                _selectionElement.style.height = height;
                var dir = GetDirection(evt.mousePosition);
                switch (dir)
                {
                    case Direction.NorthWest:
                        _selectionElement.transform.position = new Vector2(evt.mousePosition.x, evt.mousePosition.y + Y_OFFSET);
                        break;

                    case Direction.NorthEast:
                        _selectionElement.transform.position = new Vector2(evt.mousePosition.x - width, evt.mousePosition.y + Y_OFFSET);
                        break;

                    case Direction.SouthEast:
                        _selectionElement.transform.position = new Vector2(evt.mousePosition.x - width, evt.mousePosition.y - height + Y_OFFSET);
                        break;

                    case Direction.SouthWest:
                        _selectionElement.transform.position = new Vector2(evt.mousePosition.x, evt.mousePosition.y - height + Y_OFFSET);
                        break;
                }
            }
        }

        private void Design_StateChanged(DesignState oldState, DesignState newState)
        {
            if (newState == DesignState.DrawingRectangleSelect)
            {
                _selectionElement.style.display = DisplayStyle.Flex;
                _startingPoint = _design.MousePosition;
                _selectionElement.style.width = 0;
                _selectionElement.style.height = 0;
                _selectionElement.transform.position = _startingPoint;
            }
            if (newState == DesignState.Idle && oldState == DesignState.DrawingRectangleSelect)
            {
                _selectionElement.style.display = DisplayStyle.None;
                var relativeStartingPoint = _design.GetRelativePosition(_startingPoint) - _design.ToolbarOffset;
                var relativeMousePos = _design.GetRelativePosition(_design.MousePosition) - _design.ToolbarOffset;
                float minX = Mathf.Min(relativeStartingPoint.x, relativeMousePos.x) - SELECTION_PADDING;
                float minY = Mathf.Min(relativeStartingPoint.y, relativeMousePos.y) - SELECTION_PADDING;
                float maxX = Mathf.Max(relativeStartingPoint.x, relativeMousePos.x) + SELECTION_PADDING;
                float maxY = Mathf.Max(relativeStartingPoint.y, relativeMousePos.y) + SELECTION_PADDING;
                _design.UnselectAll();
                foreach (var node in _design.Nodes)
                {
                    bool isInBox = node.Rect.xMin >= minX &&
                                    node.Rect.yMin >= minY &&
                                    node.Rect.xMax <= maxX &&
                                    node.Rect.yMax <= maxY;
                    if (isInBox)
                    {
                        _design.SelectModel(node, false);
                    }
                }
                foreach (var group in _design.Groups)
                {
                    bool isInBox = group.Rect.xMin >= minX &&
                                  group.Rect.yMin >= minY &&
                                  group.Rect.xMax <= maxX &&
                                  group.Rect.yMax <= maxY;
                    if (isInBox)
                    {
                        _design.SelectModel(group, false);
                    }
                }
                foreach (var note in _design.Notes)
                {
                    bool isInBox = note.Rect.xMin >= minX &&
                                  note.Rect.yMin >= minY &&
                                  note.Rect.xMax <= maxX &&
                                  note.Rect.yMax <= maxY;
                    if (isInBox)
                    {
                        _design.SelectModel(note, false);
                    }
                }
            }
        }

        private Direction GetDirection(Vector2 currentPosition)
        {
            if (currentPosition.x > _startingPoint.x)
            {
                if (currentPosition.y < _startingPoint.y)
                {
                    return Direction.NorthEast;
                }
                else
                {
                    return Direction.SouthEast;
                }
            }
            else
            {
                if (currentPosition.y < _startingPoint.y)
                {
                    return Direction.NorthWest;
                }
                else
                {
                    return Direction.SouthWest;
                }
            }
        }
    }
}