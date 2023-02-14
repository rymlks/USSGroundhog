using UnityEngine;

namespace EasyState.Extensions
{
    public static class RectExtensions
    {
        public static Vector2 GetBottomCenter(this Rect rect)
        {
            return new Vector2(rect.position.x + rect.GetHalfWidth(), rect.position.y + rect.height);
        }

        public static Vector2 GetLeftCenter(this Rect rect)
        {
            return new Vector2(rect.position.x, rect.position.y + rect.GetHalfHeight());
        }

        public static Vector2 GetRightCenter(this Rect rect)
        {
            return new Vector2(rect.position.x + rect.width, rect.position.y + rect.GetHalfHeight());
        }

        public static Vector2 GetTopCenter(this Rect rect)
        {
            return new Vector2(rect.position.x + rect.GetHalfWidth(), rect.position.y);
        }

        private static float GetHalfHeight(this Rect rect) => rect.height / 2;

        private static float GetHalfWidth(this Rect rect) => rect.width / 2;
    }
}