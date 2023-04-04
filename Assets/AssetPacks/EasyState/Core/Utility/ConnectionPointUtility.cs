using EasyState.Core.Models;
using EasyState.Extensions;
using System;
using UnityEngine;

namespace EasyState.Core.Utility
{
    public static class ConnectionPointUtility
    {
        public static Vector2 CalculateInputPosition(Rect nodeRect, Rect connectionRect, float offset)
        {
            Vector2 pos = CalculatePosition(nodeRect, connectionRect.center, -offset);
            return pos;
        }

        public static Vector2 CalculateOutputPosition(Rect nodeRect, Rect connectionRect, float offset)
        {
            Vector2 pos = CalculatePosition(nodeRect, connectionRect.center, offset);
            return pos;
        }

        private static Vector2 CalculatePosition(Rect nodeRect, Vector2 connectionPosition, float offset)
        {
            ConnectionDirection dir = GetDirection(nodeRect.center, connectionPosition);
            Vector2 pos;
            switch (dir)
            {
                case ConnectionDirection.Left:
                    pos = nodeRect.GetLeftCenter();
                    pos.y -= offset;
                    break;

                case ConnectionDirection.Right:
                    pos = nodeRect.GetRightCenter();
                    pos.y += offset;
                    break;

                case ConnectionDirection.Up:
                    pos = nodeRect.GetTopCenter();
                    pos.x += offset;
                    break;

                case ConnectionDirection.Down:
                    pos = nodeRect.GetBottomCenter();
                    pos.x -= offset;
                    break;

                default:
                    throw new NotImplementedException();
            }
            return pos;
        }

        private static ConnectionDirection GetDirection(Vector2 nodePosition, Vector2 connectionPosition)
        {
            float xDelta = Mathf.Abs(nodePosition.x - connectionPosition.x) - 20;
            float yDelta = Mathf.Abs(nodePosition.y - connectionPosition.y);

            if (xDelta > yDelta)
            {
                if (nodePosition.x > connectionPosition.x)
                {
                    return ConnectionDirection.Left;
                }
                else
                {
                    return ConnectionDirection.Right;
                }
            }
            else
            {
                if (nodePosition.y > connectionPosition.y)
                {
                    return ConnectionDirection.Up;
                }
                else
                {
                    return ConnectionDirection.Down;
                }
            }
        }
    }
}